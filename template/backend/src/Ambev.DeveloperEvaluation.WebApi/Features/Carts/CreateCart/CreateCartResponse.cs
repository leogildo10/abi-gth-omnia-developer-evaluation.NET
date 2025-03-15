namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Response returned after creating a new cart
    /// </summary>
    public class CreateCartResponse
    {
        /// <summary>
        /// The unique identifier of the created cart
        /// </summary>
        public Guid Id { get; set; }
    }
}
