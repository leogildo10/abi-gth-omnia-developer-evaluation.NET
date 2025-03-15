namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    /// <summary>
    /// Represents a request to update an existing product
    /// </summary>
    public class UpdateProductRequest
    {
        /// <summary>
        /// The new product title (optional)
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The new product price (optional)
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// The new product description (optional)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The new product category (optional)
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// The new image URL (optional)
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
