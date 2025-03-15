namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Response returned after creating a new sale
    /// </summary>
    public class CreateSaleResponse
    {
        /// <summary>
        /// The unique identifier (ID) of the newly created sale
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The sale number
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;
    }
}
