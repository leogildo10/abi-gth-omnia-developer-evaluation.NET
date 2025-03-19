using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.Handlers.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new DeleteProductHandler(_productRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "DeleteProductHandler: Valid id deletes product")]
        public async Task Handle_WithValidId_ShouldDeleteProduct()
        {
            var productId = Guid.NewGuid();
            _productRepoMock.Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var command = new DeleteProductCommand(productId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            _productRepoMock.Verify(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
