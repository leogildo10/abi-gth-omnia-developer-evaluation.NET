namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    /// <summary>
    /// Represents a request to list products with pagination
    /// </summary>
    public class ListProductsRequest
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        public int Size { get; set; } = 10;
    }
}
