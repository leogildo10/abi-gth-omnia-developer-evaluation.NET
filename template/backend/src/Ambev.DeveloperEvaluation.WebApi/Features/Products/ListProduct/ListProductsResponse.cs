namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    /// <summary>
    /// Represents a single product in the paginated list
    /// </summary>
    public class ListProductsResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
