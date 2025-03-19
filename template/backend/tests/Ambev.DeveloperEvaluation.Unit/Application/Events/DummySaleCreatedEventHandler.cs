using System.Threading.Tasks;
using Rebus.Handlers;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers.Events
{
    public class DummySaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
    {
        public Task Handle(SaleCreatedEvent message)
        {
            return Task.CompletedTask;
        }
    }
}
