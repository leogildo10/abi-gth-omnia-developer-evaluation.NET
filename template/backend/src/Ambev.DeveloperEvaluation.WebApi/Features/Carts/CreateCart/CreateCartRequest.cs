namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Represents a request to create a new cart
    /// </summary>
    public class CreateCartRequest
    {
        /// <summary>
        /// The user ID to whom this cart belongs
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The date associated with the cart
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The list of items in this cart
        /// </summary>
        public List<CreateCartItemRequest> Items { get; set; } = new();
    }

    /// <summary>
    /// Represents an item in the cart creation request
    /// </summary>
    public class CreateCartItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
