using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Products;

/// <summary>
/// Command for deleting an existing product.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for deleting a product, 
/// including the product ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="DeleteProductResponseDto"/>.
/// </remarks>
public class DeleteProductCommand : IRequest<DeleteProductResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to be deleted.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductCommand"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the product to be deleted.</param>
    public DeleteProductCommand(Guid id) { Id = id; }
}
