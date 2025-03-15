namespace Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;

/// <summary>
/// Represents the response after retrieving a cart.
/// </summary>
public class GetCartResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the cart.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created or last updated.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the collection of items in the cart.
    /// </summary>
    public IEnumerable<GetCartItemDto> Items { get; set; } = new List<GetCartItemDto>();
}
