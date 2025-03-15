using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a shopping cart.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the cart to which this item belongs.
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product being added to the cart.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being added to the cart.
    /// </summary>
    public int Quantity { get; set; }
}
