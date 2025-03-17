namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Represents the response after retrieving a sale
    /// </summary>
    public class GetSaleResponse
    {
        /// <summary>
        /// The unique identifier (ID) of the sale
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The sale number
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
        /// The total amount for this sale
        /// </summary>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        /// The list of items in this sale
        /// </summary>
        public List<GetSaleItemResponse> Items { get; set; } = new();
    }
}
