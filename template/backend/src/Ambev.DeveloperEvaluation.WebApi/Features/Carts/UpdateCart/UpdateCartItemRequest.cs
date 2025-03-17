namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public class UpdateCartItemRequest
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
