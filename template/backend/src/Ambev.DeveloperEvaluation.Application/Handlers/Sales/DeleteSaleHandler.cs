using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales;

/// <summary>
/// Handler for <see cref="DeleteSaleCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to delete an existing sale by validating the input, 
/// and deleting the sale from the repository.
/// </remarks>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponseDto>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The sale repository instance.</param>
    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the request to delete an existing sale.
    /// </summary>
    /// <param name="request">The command containing sale ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response indicating whether the sale was successfully deleted.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the sale ID is invalid.</exception>
    public async Task<DeleteSaleResponseDto> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        // Minimal validation
        if (request.Id == Guid.Empty)
            throw new FluentValidation.ValidationException("Sale ID is required");

        await _saleRepository.DeleteAsync(request.Id, cancellationToken);
        return new DeleteSaleResponseDto { Success = true };
    }
}
