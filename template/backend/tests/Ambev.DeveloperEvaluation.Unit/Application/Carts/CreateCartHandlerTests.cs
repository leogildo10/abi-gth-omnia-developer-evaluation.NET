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
using Ambev.DeveloperEvaluation.Application.DTOs.Carts;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class CreateCartHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly CreateCartHandler _handler;

        public CreateCartHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _cartRepoMock = new Mock<ICartRepository>();
            _handler = new CreateCartHandler(_mapper, _cartRepoMock.Object, _productRepositoryMock.Object, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "CreateCartHandler: Valid command creates cart")]
        public async Task Handle_WithValidCommand_ShouldCreateCart()
        {
            var command = new CreateCartCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new System.Collections.Generic.List<CreateCartItemDto>
                {
                    new CreateCartItemDto { ProductId = Guid.NewGuid(), Quantity = 2 }
                }
            };
            _cartRepoMock.Setup(r => r.CreateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart c, CancellationToken _) => c);
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Id.Should().NotBe(Guid.Empty);
        }
    }
}
