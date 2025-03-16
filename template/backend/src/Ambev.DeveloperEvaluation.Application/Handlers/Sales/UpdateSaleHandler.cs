using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="UpdateSaleCommand"/>.
    /// Processes the update of an existing sale, saves changes, publishes a SaleModifiedEvent,
    /// and invalidates the cache for the sale record and the sales list.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;
        private readonly IRebusEventPublisher _eventPublisher;
        private readonly IRedisCacheService _cacheService;

        public UpdateSaleHandler(IMapper mapper, ISaleRepository saleRepository, IRebusEventPublisher eventPublisher, IRedisCacheService cacheService)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
        }

        public async Task<UpdateSaleResponseDto> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            // Update sale fields if provided.
            if (!string.IsNullOrEmpty(request.SaleNumber))
                sale.SaleNumber = request.SaleNumber;
            if (request.SaleDate.HasValue)
                sale.SaleDate = request.SaleDate.Value;
            if (!string.IsNullOrEmpty(request.Customer))
                sale.Customer = request.Customer;
            if (!string.IsNullOrEmpty(request.Branch))
                sale.Branch = request.Branch;

            // Update sale items logic should be implemented here.
            // For simplicity, assume items remain unchanged.
            sale.TotalSaleAmount = sale.Items.Sum(item => item.TotalAmount);

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish the SaleModifiedEvent via Rebus.
            var saleModifiedEvent = new SaleModifiedEvent
            {
                SaleId = sale.Id,
                ModifiedAt = DateTime.UtcNow
            };
            await _eventPublisher.PublishAsync(saleModifiedEvent, cancellationToken);

            // Invalida cache do registro individual e da listagem.
            await _cacheService.RemoveAsync($"sale_{request.Id}");
            await _cacheService.RemoveAsync("sales_list");

            return new UpdateSaleResponseDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber
            };
        }
    }
}
