using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts
{
    /// <summary>
    /// Handler for <see cref="GetCartCommand"/>.
    /// Processes the request to retrieve an existing cart, utilizing Redis cache.
    /// </summary>
    public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public GetCartHandler(ICartRepository cartRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<GetCartResponseDto> Handle(GetCartCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetCartCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache para o carrinho individual
            var cacheKey = $"cart_{request.Id}";

            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<GetCartResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

            var resultDto = _mapper.Map<GetCartResponseDto>(cart);

            // Salva o resultado no cache
            var serializedResult = JsonSerializer.Serialize(resultDto);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return resultDto;
        }
    }
}
