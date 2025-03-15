namespace Ambev.DeveloperEvaluation.Application.DTOs.Carts;

/// <summary>
/// Data transfer object for retrieving a cart item.
/// </summary>
public class GetCartItemDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product being added to the cart.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being added to the cart.
    /// </summary>
    public int Quantity { get; set; }
}
