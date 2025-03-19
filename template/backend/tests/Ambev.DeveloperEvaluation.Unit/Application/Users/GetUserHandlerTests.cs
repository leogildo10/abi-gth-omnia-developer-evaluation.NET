using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.Handlers.Users;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class GetUserHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserHandler _handler;

        public GetUserHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserHandler(_userRepositoryMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "GetUserHandler: Valid id returns user")]
        public async Task Handle_WithValidId_ShouldReturnUser()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@example.com", Username = "TestUser" };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            var command = new GetUserCommand(userId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }
    }
}
