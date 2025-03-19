using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Handlers.Users;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using NSubstitute;
using AutoMapper;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class CreateUserHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new CreateUserHandler(_userRepositoryMock.Object, _mapper, _passwordHasher, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "CreateUserHandler: Valid command creates user")]
        public async Task Handle_WithValidCommand_ShouldCreateUser()
        {
            var command = new CreateUserCommand
            {
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                Password = "Secret123",
                Status = UserStatus.Active,
                Role = UserRole.Customer
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);
            _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User u, CancellationToken _) => u);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            _userRepositoryMock.Verify(r => r.CreateAsync(It.Is<User>(u => u.Email == command.Email), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
