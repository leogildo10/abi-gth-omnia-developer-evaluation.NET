using Xunit;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.Handlers.Users;
using AutoMapper;
using NSubstitute;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class UpdateUserHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateUserHandler _handler;

        public UpdateUserHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateUserHandler(_userRepositoryMock.Object, _mapper, _passwordHasher, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "UpdateUserHandler: Valid command updates user and invalidates cache")]
        public async Task Handle_WithValidRequest_ShouldUpdateUser()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "OldName",
                Email = "old@example.com",
                Phone = "123456",
                Status = UserStatus.Inactive,
                Role = UserRole.Customer
            };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            var command = new UpdateUserCommand
            {
                Id = userId,
                Username = "NewName",
                Email = "new@example.com",
                Phone = "987654",
                Status = UserStatus.Active,
                Role = UserRole.Manager,
                Password = "NewSecret"
            };
            await _handler.Handle(command, CancellationToken.None);
            user.Username.Should().Be("NewName");
            user.Email.Should().Be("new@example.com");
            user.Phone.Should().Be("987654");
            user.Status.Should().Be(UserStatus.Active);
            user.Role.Should().Be(UserRole.Manager);
            _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync($"user_{userId}"), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync("users_list"), Times.Once);
        }
    }
}
