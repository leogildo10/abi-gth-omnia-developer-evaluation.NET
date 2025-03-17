using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts
{
    /// <summary>
    /// Handler for <see cref="DeleteCartCommand"/>.
    /// Processes the deletion of a cart and invalidates its cache.
    /// </summary>
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRedisCacheService _cacheService;

        public DeleteCartHandler(ICartRepository cartRepository, IRedisCacheService cacheService)
        {
            _cartRepository = cartRepository;
            _cacheService = cacheService;
        }

        public async Task<DeleteCartResponseDto> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new FluentValidation.ValidationException("Cart ID is required");

            var success = await _cartRepository.DeleteAsync(request.Id, cancellationToken);

            if (!success)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

            // Invalida o cache do carrinho específico e da listagem de carrinhos
            await _cacheService.RemoveAsync($"cart_{request.Id}");
            await _cacheService.RemoveAsync("carts_list");

            return new DeleteCartResponseDto { Success = true };
        }
    }
}
