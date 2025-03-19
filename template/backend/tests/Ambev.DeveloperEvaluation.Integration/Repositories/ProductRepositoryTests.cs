using Xunit;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
{
    public class ProductRepositoryTests : IAsyncLifetime
    {
        private DefaultContext _context;
        private ProductRepository _repository;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseNpgsql("Host=postgres;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n")
                .Options;
            _context = new DefaultContext(options);
            await _context.Database.EnsureCreatedAsync();
            _repository = new ProductRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Fact(DisplayName = "ProductRepository: Create and retrieve product successfully")]
        public async Task CreateAsync_ShouldPersistProduct()
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Title = "Prod",
                Price = 99m
            };
            var created = await _repository.CreateAsync(product);
            var fetched = await _repository.GetByIdAsync(productId);
            Assert.NotNull(created);
            Assert.NotNull(fetched);
            Assert.Equal(productId, fetched.Id);
        }
    }
}
