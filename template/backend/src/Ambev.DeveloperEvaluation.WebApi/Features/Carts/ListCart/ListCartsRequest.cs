namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    /// <summary>
    /// Represents a request to list carts with pagination
    /// </summary>
    public class ListCartsRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
