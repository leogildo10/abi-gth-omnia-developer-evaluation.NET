using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Handlers.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class DeleteSaleHandlerTests
    {
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IRebusEventPublisher> _eventPublisherMock;
        private readonly Mock<ISaleRepository> _saleRepoMock;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _eventPublisherMock = new Mock<IRebusEventPublisher>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _handler = new DeleteSaleHandler(_saleRepoMock.Object, _eventPublisherMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "DeleteSaleHandler: Valid id deletes sale")]
        public async Task Handle_WithValidId_ShouldDeleteSale()
        {
            var saleId = Guid.NewGuid();
            _saleRepoMock.Setup(r => r.DeleteAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var command = new DeleteSaleCommand(saleId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Success);
            _saleRepoMock.Verify(r => r.DeleteAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
