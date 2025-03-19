using Xunit;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
{
    public class SaleRepositoryTests : IAsyncLifetime
    {
        private DefaultContext _context;
        private SaleRepository _repository;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseNpgsql("Host=postgres;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n")
                .Options;
            _context = new DefaultContext(options);
            await _context.Database.EnsureCreatedAsync();
            _repository = new SaleRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Fact(DisplayName = "SaleRepository: Create and retrieve sale successfully")]
        public async Task CreateAsync_ShouldPersistSale()
        {
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = "S-100"
            };
            await _repository.CreateAsync(sale);
            var fetched = await _repository.GetByIdAsync(sale.Id);
            Assert.NotNull(fetched);
            Assert.Equal(sale.Id, fetched.Id);
        }
    }
}
