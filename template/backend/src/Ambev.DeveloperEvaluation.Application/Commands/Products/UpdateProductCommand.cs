using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Products;

/// <summary>
/// Command for updating an existing product.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a product, 
/// including product ID, title, price, description, category, and image URL. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UpdateProductResponseDto"/>.
/// </remarks>
public class UpdateProductCommand : IRequest<UpdateProductResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the category of the product.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the URL of the product image.
    /// </summary>
    public string? ImageUrl { get; set; }
}
