namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Represents a request to create a new sale
    /// </summary>
    public class CreateSaleRequest
    {
        /// <summary>
        /// The sale number (unique identifier for the sale in business context)
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// The date of the sale
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// The customer who made the sale
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// The branch where the sale was made
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// The list of items in the sale
        /// </summary>
        public List<CreateSaleItemRequest> Items { get; set; } = new();
    }

    /// <summary>
    /// Represents a single item in a sale
    /// </summary>
    public class CreateSaleItemRequest
    {
        /// <summary>
        /// The product ID for the item
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of this product
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price for this product
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
