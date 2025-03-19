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
    public class CreateProductHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(_mapper, _productRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "CreateProductHandler: Valid command creates product")]
        public async Task Handle_WithValidCommand_ShouldCreateProduct()
        {
            var command = new CreateProductCommand
            {
                Title = "New Product",
                Price = 100,
                Description = "A new product",
                Category = "Category1"
            };
            _productRepoMock.Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product p, CancellationToken _) => p);
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
        }
    }
}
