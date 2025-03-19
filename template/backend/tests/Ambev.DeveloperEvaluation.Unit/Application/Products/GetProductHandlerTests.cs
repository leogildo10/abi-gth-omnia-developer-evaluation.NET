using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.Handlers.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class GetProductHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly GetProductHandler _handler;

        public GetProductHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new GetProductHandler(_productRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "GetProductHandler: Valid id returns product")]
        public async Task Handle_WithValidId_ShouldReturnProduct()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Title = "Test Product", Price = 50m };
            _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            var command = new GetProductCommand(productId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact(DisplayName = "GetProductHandler: Non-existent id throws exception")]
        public async Task Handle_WhenProductNotFound_ShouldThrowKeyNotFoundException()
        {
            var productId = Guid.NewGuid();
            _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            var command = new GetProductCommand(productId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
