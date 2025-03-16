using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts
{
    /// <summary>
    /// Handler for <see cref="UpdateCartCommand"/>.
    /// Processes the update of an existing cart and invalidates related cache entries.
    /// </summary>
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public UpdateCartHandler(ICartRepository cartRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<UpdateCartResponseDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

            if (request.UserId.HasValue)
                cart.UserId = request.UserId.Value;
            if (request.Date.HasValue)
                cart.Date = request.Date.Value;

            // Lógica de atualização dos itens, se fornecida
            // (Implementar conforme a regra de negócio)

            await _cartRepository.UpdateAsync(cart, cancellationToken);

            // Invalida o cache do carrinho específico e a listagem geral
            await _cacheService.RemoveAsync($"cart_{request.Id}");
            await _cacheService.RemoveAsync("carts_list");

            return new UpdateCartResponseDto { Id = cart.Id };
        }
    }
}
