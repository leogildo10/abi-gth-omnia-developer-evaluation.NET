using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="CreateSaleCommand"/>.
    /// Processes the creation of a new sale, calculates discounts and totals,
    /// saves the sale, publishes a SaleCreatedEvent, and invalidates the sales list cache.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;
        private readonly IRebusEventPublisher _eventPublisher;
        private readonly IRedisCacheService _cacheService;

        public CreateSaleHandler(IMapper mapper, ISaleRepository saleRepository, IRebusEventPublisher eventPublisher, IRedisCacheService cacheService)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
        }

        public async Task<CreateSaleResponseDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Create the Sale entity and calculate discount and totals for each item.
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = request.SaleNumber,
                SaleDate = request.SaleDate,
                Customer = request.Customer,
                Branch = request.Branch,
                Items = request.Items.Select(i => new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = (i.Quantity >= 10) ? 0.2m : (i.Quantity >= 4 ? 0.1m : 0m),
                    TotalAmount = i.Quantity * i.UnitPrice * (1 - ((i.Quantity >= 10) ? 0.2m : (i.Quantity >= 4 ? 0.1m : 0m))),
                    Cancelled = false
                }).ToList()
            };

            // Calculate the total sale amount.
            sale.TotalSaleAmount = sale.Items.Sum(item => item.TotalAmount);

            await _saleRepository.CreateAsync(sale, cancellationToken);

            // Publish the SaleCreatedEvent via Rebus.
            var saleCreatedEvent = new SaleCreatedEvent
            {
                SaleId = sale.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _eventPublisher.PublishAsync(saleCreatedEvent, cancellationToken);

            // Invalida o cache da listagem de vendas (chave padrão: "sales_list")
            await _cacheService.RemoveAsync("sales_list");

            return new CreateSaleResponseDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber
            };
        }
    }
}
