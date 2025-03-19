using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale: Total calculation with items and discounts")]
        public void CreateSale_WithItems_ShouldCalculateTotal()
        {
            var sale = new Sale
            {
                Items = new System.Collections.Generic.List<SaleItem>
                {
                    new SaleItem { Quantity = 5, UnitPrice = 100, Discount = 0.1m },
                    new SaleItem { Quantity = 12, UnitPrice = 50, Discount = 0.2m }
                }
            };
            sale.TotalSaleAmount = sale.Items.Sum(i => i.Quantity * i.UnitPrice * (1 - i.Discount));
            Assert.Equal(5 * 100 * 0.9m + 12 * 50 * 0.8m, sale.TotalSaleAmount);
        }
    }
}
