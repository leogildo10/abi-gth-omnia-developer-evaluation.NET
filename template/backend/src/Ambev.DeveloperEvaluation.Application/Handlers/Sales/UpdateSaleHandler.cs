using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="UpdateSaleCommand"/>.
    /// Processes the update of an existing sale, saves changes and publishes a SaleModifiedEvent.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;
        private readonly IRebusEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="saleRepository">The sale repository instance.</param>
        /// <param name="eventPublisher">The event publisher instance.</param>
        public UpdateSaleHandler(IMapper mapper, ISaleRepository saleRepository, IRebusEventPublisher eventPublisher)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the request to update an existing sale.
        /// </summary>
        /// <param name="request">The command containing sale update details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response DTO containing the updated sale details.</returns>
        /// <exception cref="FluentValidation.ValidationException">Thrown when validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the sale is not found.</exception>
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

            return new UpdateSaleResponseDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber
            };
        }
    }
}
