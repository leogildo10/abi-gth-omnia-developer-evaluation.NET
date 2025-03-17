namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
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
