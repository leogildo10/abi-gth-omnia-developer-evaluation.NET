namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Represents a request to update an existing sale
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// The new sale number (optional)
        /// </summary>
        public string? SaleNumber { get; set; }

        /// <summary>
        /// The new sale date (optional)
        /// </summary>
        public DateTime? SaleDate { get; set; }

        /// <summary>
        /// The new customer name (optional)
        /// </summary>
        public string? Customer { get; set; }

        /// <summary>
        /// The new branch (optional)
        /// </summary>
        public string? Branch { get; set; }

        /// <summary>
        /// Optional updated list of items
        /// </summary>
        public List<UpdateSaleItemRequest>? Items { get; set; }
    }

    /// <summary>
    /// Represents an item in the sale that is being updated
    /// </summary>
    public class UpdateSaleItemRequest
    {
        public Guid? Id { get; set; } // if updating existing item
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
