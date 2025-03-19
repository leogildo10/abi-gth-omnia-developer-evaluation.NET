using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.Handlers.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class GetCartHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly GetCartHandler _handler;

        public GetCartHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _cartRepoMock = new Mock<ICartRepository>();
            _handler = new GetCartHandler(_cartRepoMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "GetCartHandler: Valid id returns cart")]
        public async Task Handle_WithValidId_ShouldReturnCart()
        {
            var cartId = Guid.NewGuid();
            var cart = new Cart { Id = cartId, UserId = Guid.NewGuid() };
            _cartRepoMock.Setup(r => r.GetByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);
            var command = new GetCartCommand(cartId);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(cartId, result.Id);
        }

        [Fact(DisplayName = "GetCartHandler: Non-existent id throws exception")]
        public async Task Handle_WhenCartNotFound_ShouldThrowKeyNotFoundException()
        {
            var cartId = Guid.NewGuid();
            _cartRepoMock.Setup(r => r.GetByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);
            var command = new GetCartCommand(cartId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
