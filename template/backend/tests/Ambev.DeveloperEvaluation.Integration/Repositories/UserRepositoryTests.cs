using Xunit;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
{
    public class UserRepositoryTests : IAsyncLifetime
    {
        private DefaultContext _context;
        private IUserRepository _repository;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseNpgsql("Host=postgres;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n")
                .Options;
            _context = new DefaultContext(options);
            await _context.Database.EnsureCreatedAsync();
            _repository = new UserRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Fact(DisplayName = "UserRepository: Create and retrieve user successfully")]
        public async Task CreateAsync_WithValidUser_ShouldPersist()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Email = "test@example.com"
            };
            var created = await _repository.CreateAsync(user);
            var fetched = await _repository.GetByIdAsync(userId);
            Assert.NotNull(created);
            Assert.NotNull(fetched);
            Assert.Equal(userId, fetched.Id);
        }
    }
}
