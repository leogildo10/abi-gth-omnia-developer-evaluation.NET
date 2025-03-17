using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using OneOf.Types;

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
        private readonly IRedisCacheService _cacheService;

        public DeleteSaleHandler(ISaleRepository saleRepository, IRebusEventPublisher eventPublisher, IRedisCacheService cacheService)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
        }

        public async Task<DeleteSaleResponseDto> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new FluentValidation.ValidationException("Sale ID is required");

            var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);

            if (!success)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            // Publish the SaleCancelledEvent via Rebus.
            var saleCancelledEvent = new SaleCancelledEvent
            {
                SaleId = request.Id,
                CancelledAt = DateTime.UtcNow
            };
            await _eventPublisher.PublishAsync(saleCancelledEvent, cancellationToken);

            // Invalida cache do registro individual e da listagem.
            await _cacheService.RemoveAsync($"sale_{request.Id}");
            await _cacheService.RemoveAsync("sales_list");

            return new DeleteSaleResponseDto { Success = true };
        }
    }
}
