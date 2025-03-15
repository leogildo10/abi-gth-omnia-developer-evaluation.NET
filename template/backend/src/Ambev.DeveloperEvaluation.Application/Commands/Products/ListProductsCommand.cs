using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Products;

/// <summary>
/// Command for listing products with pagination.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for listing products, 
/// including the page number and page size for pagination. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListProductsResponseDto"/>.
/// </remarks>
public class ListProductsCommand : IRequest<ListProductsResponseDto>
{
    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the page size for pagination.
    /// </summary>
    public int Size { get; set; }
}
