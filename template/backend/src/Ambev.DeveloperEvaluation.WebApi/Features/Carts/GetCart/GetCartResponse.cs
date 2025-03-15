namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    /// <summary>
    /// Response model for retrieving a cart
    /// </summary>
    public class GetCartResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<GetCartItemResponse> Items { get; set; } = new();
    }

    public class GetCartItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
