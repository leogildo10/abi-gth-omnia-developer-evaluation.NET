using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    /// <summary>
    /// Implementation of IEventPublisher using Rebus.
    /// </summary>
    public class RebusEventPublisher : IRebusEventPublisher
    {
        private readonly IBus _bus;

        /// <summary>
        /// Initializes a new instance of RebusEventPublisher.
        /// </summary>
        /// <param name="bus">The Rebus IBus instance.</param>
        public RebusEventPublisher(IBus bus)
        {
            _bus = bus;
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            // Publish the event via Rebus
            await _bus.Publish(@event);
        }
    }
}
