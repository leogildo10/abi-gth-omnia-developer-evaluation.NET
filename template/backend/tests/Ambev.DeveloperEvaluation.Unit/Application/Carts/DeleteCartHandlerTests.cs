using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.Handlers.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class DeleteCartHandlerTests
    {
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly DeleteCartHandler _handler;

        public DeleteCartHandlerTests()
        {
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _cartRepoMock = new Mock<ICartRepository>();
            _handler = new DeleteCartHandler(_cartRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "DeleteCartHandler: Valid id deletes cart")]
        public async Task Handle_WithValidId_ShouldDeleteCart()
        {
            var cartId = Guid.NewGuid();
            _cartRepoMock.Setup(r => r.DeleteAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var command = new DeleteCartCommand(cartId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            _cartRepoMock.Verify(r => r.DeleteAsync(cartId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
