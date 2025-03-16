using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts
{
    /// <summary>
    /// Handler for <see cref="ListCartsCommand"/>.
    /// Processes the request to list carts with pagination using Redis as cache.
    /// </summary>
    public class ListCartsHandler : IRequestHandler<ListCartsCommand, ListCartsResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public ListCartsHandler(ICartRepository cartRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ListCartsResponseDto> Handle(ListCartsCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListCartsCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache com base na paginação
            var cacheKey = $"carts_list_page_{request.Page}_size_{request.Size}";

            // Tenta obter os dados do cache
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<ListCartsResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var query = _cartRepository.GetAllCarts();
            var paginatedList = await PaginatedList<Cart>.CreateAsync(query, request.Page, request.Size);

            var cartsDto = paginatedList.Select(c => _mapper.Map<GetCartResponseDto>(c)).ToList();

            var result = new ListCartsResponseDto
            {
                Carts = cartsDto,
                Page = paginatedList.CurrentPage,
                TotalCount = paginatedList.TotalCount
            };

            // Salva o resultado no cache com TTL de 5 minutos
            var serializedResult = JsonSerializer.Serialize(result);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return result;
        }
    }
}
