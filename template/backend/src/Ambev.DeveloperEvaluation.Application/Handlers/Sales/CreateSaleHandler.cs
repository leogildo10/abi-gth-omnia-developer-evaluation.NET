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
    /// saves the sale and publishes a SaleCreatedEvent.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;
        private readonly IRebusEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="saleRepository">The sale repository instance.</param>
        /// <param name="eventPublisher">The event publisher instance.</param>
        public CreateSaleHandler(IMapper mapper, ISaleRepository saleRepository, IRebusEventPublisher eventPublisher)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the request to create a new sale.
        /// </summary>
        /// <param name="request">The command containing sale details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response DTO containing the created sale details.</returns>
        /// <exception cref="FluentValidation.ValidationException">Thrown when validation fails.</exception>
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

            return new CreateSaleResponseDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber
            };
        }
    }
}
