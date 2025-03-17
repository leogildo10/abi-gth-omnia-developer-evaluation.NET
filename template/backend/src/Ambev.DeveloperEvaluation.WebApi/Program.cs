using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using Ambev.DeveloperEvaluation.Application.Services;
using StackExchange.Redis;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Rebus.Config;
using RabbitMQ.Client;

namespace Ambev.DeveloperEvaluation.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting web application");

                var builder = WebApplication.CreateBuilder(args);
                builder.AddDefaultLogging();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.AddBasicHealthChecks();
                builder.Services.AddSwaggerGen();

                // Configuração do EF Core com PostgreSQL
                builder.Services.AddDbContext<DefaultContext>(options =>
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString("PostgresConnection"),
                        b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                    )
                );

                // Inicializa a exchange, a fila e o binding no RabbitMQ de forma assíncrona
                Task.Run(async () =>
                {
                    await InitializeRabbitMqQueue(builder.Configuration.GetConnectionString("RabbitMQ"));
                }).GetAwaiter().GetResult();

                // Configuração do Rebus para enviar mensagens diretamente para a fila "ambev.sales.queue"
                builder.Services
                    .AddRebus((configure, provider) =>
                    {
                        return configure
                            .Transport(t => t.UseRabbitMq(
                                builder.Configuration.GetConnectionString("RabbitMQ"),
                                "ambev.sales.queue")
                                .ExchangeNames("ambev.sales.exchange"))
                            .Options(o => o.SetNumberOfWorkers(0));
                    });

                builder.Services.AddJwtAuthentication(builder.Configuration);

                builder.RegisterDependencies();

                builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
                builder.Services.AddAutoMapper(typeof(MappingProfile));

                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(ApplicationLayer).Assembly,
                        typeof(Program).Assembly
                    );
                });

                builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                // Configuração do Redis
                builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
                    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
                builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

                // Registro do publicador de eventos via Rebus
                builder.Services.AddScoped<IRebusEventPublisher, RebusEventPublisher>();

                var app = builder.Build();

                app.UseMiddleware<ValidationExceptionMiddleware>();
                app.UseMiddleware<ExceptionHandlingMiddleware>();

                if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseBasicHealthChecks();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Inicializa a exchange, a fila e o binding no RabbitMQ.
        /// </summary>
        /// <param name="connectionString">A connection string do RabbitMQ.</param>
        /// <returns>Uma Task representando a operação assíncrona.</returns>
        private static async Task InitializeRabbitMqQueue(string connectionString)
        {
            var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

            // Usa os métodos assíncronos para criar conexão e canal.
            using var connection = await factory.CreateConnectionAsync();
            // Se o método assíncrono para criar canal existir, use-o; caso contrário, utilize o método síncrono.
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "ambev.sales.exchange",
                type: "direct",
                durable: true,
                autoDelete: false
            );

            await channel.QueueDeclareAsync(
                queue: "ambev.sales.queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: "ambev.sales.queue",
                exchange: "ambev.sales.exchange",
                routingKey: "ambev.sales.queue"
            );
        }
    }
}
