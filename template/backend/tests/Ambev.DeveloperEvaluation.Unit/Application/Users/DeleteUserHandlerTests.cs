using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.Handlers.Users;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _handler = new DeleteUserHandler(_userRepositoryMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "DeleteUserHandler: Valid id deletes user")]
        public async Task Handle_WithValidId_ShouldDeleteUser()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.DeleteAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var command = new DeleteUserCommand(userId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            _userRepositoryMock.Verify(r => r.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
