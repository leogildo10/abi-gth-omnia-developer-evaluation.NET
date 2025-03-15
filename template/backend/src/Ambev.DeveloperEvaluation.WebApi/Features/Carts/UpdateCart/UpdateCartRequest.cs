namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Represents a request to update an existing cart
    /// </summary>
    public class UpdateCartRequest
    {
        /// <summary>
        /// The new user ID (optional)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// The new date (optional)
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// The updated list of items (optional)
        /// </summary>
        public List<UpdateCartItemRequest>? Items { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
