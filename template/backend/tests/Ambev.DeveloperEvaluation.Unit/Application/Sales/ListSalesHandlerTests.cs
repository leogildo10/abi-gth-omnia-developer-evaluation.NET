using Xunit;
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
    public class ListSalesHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<ISaleRepository> _saleRepoMock;
        private readonly ListSalesHandler _handler;

        public ListSalesHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _handler = new ListSalesHandler(_saleRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "ListSalesHandler: Returns paginated list of sales")]
        public async Task Handle_WithValidRequest_ShouldReturnPaginatedSales()
        {
            var sales = new[]
            {
                new Sale { Id = Guid.NewGuid(), SaleNumber = "S-001" },
                new Sale { Id = Guid.NewGuid(), SaleNumber = "S-002" }
            }.AsQueryable();
            _saleRepoMock.Setup(r => r.GetAllSales()).Returns(sales);
            var command = new ListSalesCommand { Page = 1, Size = 10 };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Sales.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }
    }
}
