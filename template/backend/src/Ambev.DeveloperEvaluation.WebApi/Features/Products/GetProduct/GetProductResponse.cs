namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    /// <summary>
    /// Represents the response after retrieving a product
    /// </summary>
    public class GetProductResponse
    {
        /// <summary>
        /// The unique identifier (ID) of the product
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The product title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The product price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The product description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The product category
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// The product image URL
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }
}
