using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="DeleteSaleCommand"/>.
    /// Processes the deletion of a sale and publishes a SaleCancelledEvent.
    /// </summary>
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponseDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IRebusEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository instance.</param>
        /// <param name="eventPublisher">The event publisher instance.</param>
        public DeleteSaleHandler(ISaleRepository saleRepository, IRebusEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the request to delete an existing sale.
        /// </summary>
        /// <param name="request">The command containing the sale ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response DTO indicating deletion success.</returns>
        /// <exception cref="FluentValidation.ValidationException">Thrown when the sale ID is invalid.</exception>
        public async Task<DeleteSaleResponseDto> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new FluentValidation.ValidationException("Sale ID is required");

            await _saleRepository.DeleteAsync(request.Id, cancellationToken);

            // Publish the SaleCancelledEvent via Rebus.
            var saleCancelledEvent = new SaleCancelledEvent
            {
                SaleId = request.Id,
                CancelledAt = DateTime.UtcNow
            };
            await _eventPublisher.PublishAsync(saleCancelledEvent, cancellationToken);

            return new DeleteSaleResponseDto { Success = true };
        }
    }
}
