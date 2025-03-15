using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Products;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products;

/// <summary>
/// Handler for <see cref="DeleteProductCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to delete an existing product by validating the input, 
/// and deleting the product from the repository.
/// </remarks>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponseDto>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductHandler"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository instance.</param>
    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the request to delete an existing product.
    /// </summary>
    /// <param name="request">The command containing product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response indicating whether the product was successfully deleted.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the product ID is invalid.</exception>
    public async Task<DeleteProductResponseDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new FluentValidation.ValidationException("Product ID is required");

        await _productRepository.DeleteAsync(request.Id, cancellationToken);
        return new DeleteProductResponseDto { Success = true };
    }
}
