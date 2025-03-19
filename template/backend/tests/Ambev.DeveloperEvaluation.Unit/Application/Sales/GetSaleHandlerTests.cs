using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Handlers.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetSaleHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<ISaleRepository> _saleRepoMock;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _handler = new GetSaleHandler(_saleRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "GetSaleHandler: Valid id returns sale")]
        public async Task Handle_WithValidId_ShouldReturnSale()
        {
            var saleId = Guid.NewGuid();
            var sale = new Sale { Id = saleId, SaleNumber = "S-001" };
            _saleRepoMock.Setup(r => r.GetByIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);
            var command = new GetSaleCommand(saleId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(saleId, result.Id);
        }
    }
}
