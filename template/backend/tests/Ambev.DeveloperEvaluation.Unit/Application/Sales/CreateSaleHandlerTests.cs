using Xunit;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Handlers.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class CreateSaleHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IRebusEventPublisher> _eventPublisherMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ISaleRepository> _saleRepoMock;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _eventPublisherMock = new Mock<IRebusEventPublisher>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _handler = new CreateSaleHandler(_mapper, _saleRepoMock.Object, _productRepositoryMock.Object, _eventPublisherMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "CreateSaleHandler: Valid command creates sale")]
        public async Task Handle_WithValidRequest_ShouldCreateSale()
        {
            var command = new CreateSaleCommand
            {
                SaleNumber = "S-001",
                Items = new System.Collections.Generic.List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto { ProductId = Guid.NewGuid(), Quantity = 5, UnitPrice = 100 }
                }
            };

            _saleRepoMock.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .Returns((Sale s, CancellationToken _) => s);
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Id.Should().NotBe(Guid.Empty);
        }
    }
}
