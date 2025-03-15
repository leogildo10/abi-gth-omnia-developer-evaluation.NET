namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Represents a request to list sales with pagination
    /// </summary>
    public class ListSalesRequest
    {
        /// <summary>
        /// Gets or sets the page number for pagination
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the page size for pagination
        /// </summary>
        public int Size { get; set; } = 10;
    }
}
