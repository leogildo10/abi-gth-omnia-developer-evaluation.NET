using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM
{
    public static class DatabaseInitializer
    {
        public static async Task SeedAsync(DefaultContext context)
        {
            // Se já houver dados em alguma das tabelas, não semear novamente
            if (await context.Users.AnyAsync() ||
                await context.Products.AnyAsync() ||
                await context.Carts.AnyAsync() ||
                await context.Sales.AnyAsync())
            {
                return;
            }

            var random = new Random();

            // Gerar 50 usuários com senhas hasheadas usando BCrypt.Net
            var users = Enumerable.Range(1, 50).Select(i => new User
            {
                Id = Guid.NewGuid(),
                Email = $"user{i}@example.com",
                Username = $"user{i}",
                // Usa BCrypt para hashear a senha
                Password = BCrypt.Net.BCrypt.HashPassword($"password{i}"),
                Status = UserStatus.Active,
                Role = (i % 2 == 0) ? UserRole.Customer : UserRole.Manager,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            }).ToList();

            context.Users.AddRange(users);

            // Gerar 50 produtos
            var products = Enumerable.Range(1, 50).Select(i => new Product
            {
                Id = Guid.NewGuid(),
                Title = $"Product {i}",
                Price = Math.Round((decimal)(random.NextDouble() * 100 + 10), 2), // Preço entre 10 e 110
                Description = $"Description for product {i}",
                Category = $"Category {i % 5 + 1}",
                ImageUrl = $"https://example.com/product{i}.jpg"
            }).ToList();

            context.Products.AddRange(products);

            // Gerar 50 carrinhos, cada um associado a um usuário aleatório com 1 a 3 itens
            var carts = Enumerable.Range(1, 50).Select(i =>
            {
                var user = users[random.Next(users.Count)];
                int itemsCount = random.Next(1, 4); // entre 1 e 3 itens
                var cartItems = Enumerable.Range(1, itemsCount).Select(j => new CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = products[random.Next(products.Count)].Id,
                    Quantity = random.Next(1, 5) // quantidade entre 1 e 4
                }).ToList();

                return new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Date = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                    Items = cartItems
                };
            }).ToList();

            context.Carts.AddRange(carts);

            // Gerar 50 vendas, cada uma associada a um usuário aleatório e com 1 a 3 itens de venda
            var sales = Enumerable.Range(1, 50).Select(i =>
            {
                var user = users[random.Next(users.Count)];
                int itemsCount = random.Next(1, 4); // entre 1 e 3 itens
                var saleItems = Enumerable.Range(1, itemsCount).Select(j =>
                {
                    var product = products[random.Next(products.Count)];
                    int quantity = random.Next(1, 15); // quantidade entre 1 e 14
                    decimal discount = 0m;
                    if (quantity >= 10 && quantity <= 20)
                        discount = 0.2m;
                    else if (quantity >= 4)
                        discount = 0.1m;

                    return new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        Quantity = quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalAmount = quantity * product.Price * (1 - discount),
                        Cancelled = false
                    };
                }).ToList();

                return new Sale
                {
                    Id = Guid.NewGuid(),
                    SaleNumber = $"S-{i:D3}",
                    SaleDate = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                    Customer = user.Username,
                    Branch = $"Store {random.Next(1, 5)}",
                    Items = saleItems,
                    TotalSaleAmount = saleItems.Sum(si => si.TotalAmount)
                };
            }).ToList();

            context.Sales.AddRange(sales);

            await context.SaveChangesAsync();
        }
    }
}
