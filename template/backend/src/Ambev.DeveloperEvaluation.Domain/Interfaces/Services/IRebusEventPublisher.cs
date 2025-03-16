namespace Ambev.DeveloperEvaluation.Domain.Interfaces.Services
{
    /// <summary>
    /// Defines a contract for publishing events.
    /// </summary>
    public interface IRebusEventPublisher
    {
        /// <summary>
        /// Publishes an event asynchronously.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event instance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
    }
}
