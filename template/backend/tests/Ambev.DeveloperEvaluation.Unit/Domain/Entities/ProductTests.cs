using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class ProductTests
    {
        [Fact(DisplayName = "Product: Create with valid data sets properties correctly")]
        public void CreateProduct_WithValidData_ShouldSetProperties()
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Title = "Product A",
                Price = 150m,
                Description = "A great product",
                Category = "Category1"
            };

            Assert.Equal(productId, product.Id);
            Assert.Equal("Product A", product.Title);
            Assert.Equal(150m, product.Price);
            Assert.Equal("A great product", product.Description);
            Assert.Equal("Category1", product.Category);
        }
    }
}
