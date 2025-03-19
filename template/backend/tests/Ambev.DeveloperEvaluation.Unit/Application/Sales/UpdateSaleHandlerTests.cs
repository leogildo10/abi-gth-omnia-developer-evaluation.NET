using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Handlers.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class UpdateSaleHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IRebusEventPublisher> _eventPublisherMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ISaleRepository> _saleRepoMock;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _eventPublisherMock = new Mock<IRebusEventPublisher>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _handler = new UpdateSaleHandler(_mapper, _saleRepoMock.Object, _productRepositoryMock.Object, _eventPublisherMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "UpdateSaleHandler: Valid command updates sale")]
        public async Task Handle_WithValidRequest_ShouldUpdateSale()
        {
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                SaleNumber = "S-001",
                Customer = "Old Customer",
                Branch = "Old Branch",
                Items = new System.Collections.Generic.List<SaleItem>
                {
                    new SaleItem { Quantity = 5, UnitPrice = 100, Discount = 0.1m }
                }
            };
            _saleRepoMock.Setup(r => r.GetByIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);
            var command = new UpdateSaleCommand
            {
                Id = saleId,
                SaleNumber = "S-001-Updated",
                SaleDate = DateTime.UtcNow,
                Customer = "New Customer",
                Branch = "New Branch"
            };
            await _handler.Handle(command, CancellationToken.None);
            sale.SaleNumber.Should().Be("S-001-Updated");
            sale.Customer.Should().Be("New Customer");
            sale.Branch.Should().Be("New Branch");
            _saleRepoMock.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
