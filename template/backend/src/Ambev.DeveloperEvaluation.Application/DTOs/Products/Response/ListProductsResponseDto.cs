namespace Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;

/// <summary>
/// Represents the response after listing products with pagination.
/// </summary>
public class ListProductsResponseDto
{
    /// <summary>
    /// Gets or sets the collection of products.
    /// </summary>
    public IEnumerable<GetProductResponseDto> Products { get; set; } = new List<GetProductResponseDto>();

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the total count of products.
    /// </summary>
    public int TotalCount { get; set; }
}
