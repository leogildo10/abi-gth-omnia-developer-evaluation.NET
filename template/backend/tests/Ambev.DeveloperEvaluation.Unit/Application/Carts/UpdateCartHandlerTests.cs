using Xunit;
using Moq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.Handlers.Carts;
using AutoMapper;
using NSubstitute;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class UpdateCartHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly UpdateCartHandler _handler;

        public UpdateCartHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cartRepoMock = new Mock<ICartRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _handler = new UpdateCartHandler(_mapper, _cartRepoMock.Object, _productRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "UpdateCartHandler: Valid command updates cart")]
        public async Task Handle_WithValidRequest_ShouldUpdateCart()
        {
            var cartId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow
            };
            _cartRepoMock.Setup(r => r.GetByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);
            _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product { Id = Guid.NewGuid() });
            var command = new UpdateCartCommand
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Items = new System.Collections.Generic.List<UpdateCartItemDto>
                {
                    new UpdateCartItemDto { ProductId = Guid.NewGuid(), Quantity = 3 }
                }
            };
            await _handler.Handle(command, CancellationToken.None);
            _cartRepoMock.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
