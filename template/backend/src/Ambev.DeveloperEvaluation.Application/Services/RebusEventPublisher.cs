using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    /// <summary>
    /// Service that publishes events directly to a RabbitMQ exchange.
    /// </summary>
    public class RebusEventPublisher : IRebusEventPublisher
    {
        private readonly ILogger<RebusEventPublisher> _logger;
        private readonly string _connectionString;

        public RebusEventPublisher(IConfiguration configuration, ILogger<RebusEventPublisher> logger)
        {
            _connectionString = configuration.GetConnectionString("RabbitMQ") ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            _logger.LogInformation("RebusEventPublisher instanciado com sucesso.");
        }

        /// <summary>
        /// Publishes the given event directly to the "ambev.sales.exchange" exchange,
        /// routing the message to the "ambev.sales.queue" using BasicPublish.
        /// </summary>
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Tentando publicar evento: {EventType} - {@Event}", typeof(TEvent).Name, @event);
            try
            {
                var factory = new ConnectionFactory { Uri = new Uri(_connectionString) };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                var properties = new BasicProperties();
                properties.ContentType = "text/plain";
                properties.DeliveryMode = (DeliveryModes)2;
                properties.Headers = new Dictionary<string, object?>
                {
                    { "rbs2-content-type", "application/json" },
                    { "rbs2-msg-id", Guid.NewGuid().ToString() },
                    { "rbs2-msg-type", typeof(TEvent).AssemblyQualifiedName }
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
                // Publica a mensagem na exchange "ambev.sales.exchange" com a routing key "ambev.sales.queue"
                await channel.BasicPublishAsync(
                    exchange: "ambev.sales.exchange",
                    routingKey: "ambev.sales.queue",
                    true,
                    properties,
                    body: body);
                _logger.LogInformation("Evento {EventType} publicado com sucesso para ambev.sales.exchange.", typeof(TEvent).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar evento {EventType}", typeof(TEvent).Name);
                throw;
            }
        }
    }
}
