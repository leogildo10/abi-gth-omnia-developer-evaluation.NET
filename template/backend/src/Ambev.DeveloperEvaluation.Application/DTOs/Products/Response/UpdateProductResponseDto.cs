namespace Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;

/// <summary>
/// Represents the response after updating a product.
/// </summary>
public class UpdateProductResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
