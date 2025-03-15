namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    /// <summary>
    /// Response returned after updating a product
    /// </summary>
    public class UpdateProductResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
