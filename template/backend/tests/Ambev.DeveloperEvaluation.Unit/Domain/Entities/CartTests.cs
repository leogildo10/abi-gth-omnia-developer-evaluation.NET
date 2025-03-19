using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class CartTests
    {
        [Fact(DisplayName = "Cart: Create with valid data sets properties correctly")]
        public void CreateCart_ShouldSetProperties()
        {
            var cartId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow
            };
            Assert.Equal(cartId, cart.Id);
        }
    }
}
