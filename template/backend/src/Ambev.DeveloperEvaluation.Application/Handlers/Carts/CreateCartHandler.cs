using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts
{
    /// <summary>
    /// Handler for <see cref="CreateCartCommand"/>.
    /// Processes the creation of a new cart, saves it in the repository and invalidates the cart list cache.
    /// </summary>
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IRedisCacheService _cacheService;

        public CreateCartHandler(IMapper mapper, ICartRepository cartRepository, IRedisCacheService cacheService)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _cacheService = cacheService;
        }

        public async Task<CreateCartResponseDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Date = request.Date,
                Items = request.Items.Select(i => new CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _cartRepository.CreateAsync(cart, cancellationToken);

            // Invalida o cache da listagem de carrinhos (chave padrão "carts_list")
            await _cacheService.RemoveAsync("carts_list");

            return new CreateCartResponseDto { Id = cart.Id };
        }
    }
}
