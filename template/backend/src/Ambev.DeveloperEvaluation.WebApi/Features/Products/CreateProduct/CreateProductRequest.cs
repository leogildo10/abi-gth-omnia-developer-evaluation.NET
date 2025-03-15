namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    /// <summary>
    /// Represents a request to create a new product
    /// </summary>
    public class CreateProductRequest
    {
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
        /// The image URL of the product
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }
}
