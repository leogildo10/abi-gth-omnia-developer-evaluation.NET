namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    /// <summary>
    /// Response returned after creating a new product
    /// </summary>
    public class CreateProductResponse
    {
        /// <summary>
        /// The unique identifier of the created product
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The product title
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
