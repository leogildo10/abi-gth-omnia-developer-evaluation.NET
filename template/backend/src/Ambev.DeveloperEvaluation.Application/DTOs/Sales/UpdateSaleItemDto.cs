namespace Ambev.DeveloperEvaluation.Application.DTOs.Sales;

/// <summary>
/// Data transfer object for updating a sale item.
/// </summary>
public class UpdateSaleItemDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the existing sale item.
    /// </summary>
    public Guid Id { get; set; } // Id do item existente

    /// <summary>
    /// Gets or sets the unique identifier of the product being sold.
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal? UnitPrice { get; set; }
}
