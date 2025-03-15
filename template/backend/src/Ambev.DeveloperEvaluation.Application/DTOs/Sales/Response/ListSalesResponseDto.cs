namespace Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;

/// <summary>
/// Represents the response after listing sales with pagination.
/// </summary>
public class ListSalesResponseDto
{
    /// <summary>
    /// Gets or sets the collection of sales.
    /// </summary>
    public IEnumerable<GetSaleResponseDto> Sales { get; set; } = new List<GetSaleResponseDto>();

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the total count of sales.
    /// </summary>
    public int TotalCount { get; set; }
}
