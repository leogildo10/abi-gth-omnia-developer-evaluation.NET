using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales;

/// <summary>
/// Handler for <see cref="UpdateSaleCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to update an existing sale by validating the input, 
/// retrieving the sale from the repository, updating the sale's details, and saving the changes.
/// </remarks>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="saleRepository">The sale repository instance.</param>
    public UpdateSaleHandler(IMapper mapper, ISaleRepository saleRepository)
    {
        _mapper = mapper;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the request to update an existing sale.
    /// </summary>
    /// <param name="request">The command containing sale update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the updated sale details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
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

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.SaleNumber))
            sale.SaleNumber = request.SaleNumber;
        if (request.SaleDate.HasValue)
            sale.SaleDate = request.SaleDate.Value;
        if (!string.IsNullOrEmpty(request.Customer))
            sale.Customer = request.Customer;
        if (!string.IsNullOrEmpty(request.Branch))
            sale.Branch = request.Branch;

        // For items, an update logic should be applied (simply omitted here for brevity)
        // Recalculate the total
        sale.TotalSaleAmount = sale.Items.Sum(item => item.TotalAmount);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new UpdateSaleResponseDto
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };
    }
}
