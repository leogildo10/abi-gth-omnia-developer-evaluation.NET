using Xunit;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.Handlers.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class ListCartsHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly ListCartsHandler _handler;

        public ListCartsHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _cartRepoMock = new Mock<ICartRepository>();
            _handler = new ListCartsHandler(_cartRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "ListCartsHandler: Returns paginated list of carts")]
        public async Task Handle_WithValidRequest_ShouldReturnPaginatedCarts()
        {
            var carts = new[]
            {
                new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() },
                new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }
            }.AsQueryable();
            _cartRepoMock.Setup(r => r.GetAllCarts()).Returns(carts);
            var command = new ListCartsCommand { Page = 1, Size = 10 };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Carts.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }
    }
}
