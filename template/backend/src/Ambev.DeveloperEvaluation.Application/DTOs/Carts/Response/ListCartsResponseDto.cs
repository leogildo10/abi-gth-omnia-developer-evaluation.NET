namespace Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;

/// <summary>
/// Represents the response after listing carts with pagination.
/// </summary>
public class ListCartsResponseDto
{
    /// <summary>
    /// Gets or sets the collection of carts.
    /// </summary>
    public IEnumerable<GetCartResponseDto> Carts { get; set; } = new List<GetCartResponseDto>();

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the total count of carts.
    /// </summary>
    public int TotalCount { get; set; }
}
