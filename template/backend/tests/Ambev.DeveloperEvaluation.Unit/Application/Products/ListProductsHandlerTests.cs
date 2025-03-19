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
    public class ListProductsHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly ListProductsHandler _handler;

        public ListProductsHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new ListProductsHandler(_productRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "ListProductsHandler: Returns paginated list of products")]
        public async Task Handle_WithValidRequest_ShouldReturnPaginatedProducts()
        {
            var products = new[]
            {
                new Product { Id = Guid.NewGuid(), Title = "A", Price = 100 },
                new Product { Id = Guid.NewGuid(), Title = "B", Price = 200 }
            }.AsQueryable();
            _productRepoMock.Setup(r => r.GetAllProducts()).Returns(products);
            var command = new ListProductsCommand { Page = 1, Size = 10 };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Products.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }
    }
}
