using Xunit;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.Handlers.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class UpdateProductHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler(_mapper, _productRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "UpdateProductHandler: Valid command updates product")]
        public async Task Handle_WithValidRequest_ShouldUpdateProduct()
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Title = "Old Title",
                Price = 100m,
                Description = "Old Desc",
                Category = "Old Cat",
                ImageUrl = "old.jpg"
            };
            _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            var command = new UpdateProductCommand
            {
                Id = productId,
                Title = "New Title",
                Price = 150m,
                Description = "New Desc",
                Category = "New Cat",
                ImageUrl = "new.jpg"
            };
            await _handler.Handle(command, CancellationToken.None);
            product.Title.Should().Be("New Title");
            product.Price.Should().Be(150m);
            product.Description.Should().Be("New Desc");
            product.Category.Should().Be("New Cat");
            product.ImageUrl.Should().Be("new.jpg");
            _productRepoMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
