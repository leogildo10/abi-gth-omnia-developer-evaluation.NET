using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products
{
    /// <summary>
    /// Handler for <see cref="ListProductsCommand"/>.
    /// Processes the request to list products with pagination, using Redis cache.
    /// </summary>
    public class ListProductsHandler : IRequestHandler<ListProductsCommand, ListProductsResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public ListProductsHandler(IProductRepository productRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ListProductsResponseDto> Handle(ListProductsCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListProductsCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache para a listagem de produtos baseada na paginação.
            var cacheKey = $"products_list_page_{request.Page}_size_{request.Size}";

            // Tenta obter o resultado do cache.
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<ListProductsResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var query = _productRepository.GetAllProducts();
            var paginatedList = await PaginatedList<Product>.CreateAsync(query, request.Page, request.Size);
            var productsDto = paginatedList.Select(p => _mapper.Map<GetProductResponseDto>(p)).ToList();

            var result = new ListProductsResponseDto
            {
                Products = productsDto,
                Page = paginatedList.CurrentPage,
                TotalCount = paginatedList.TotalCount
            };

            // Serializa e salva no cache com TTL de 5 minutos.
            var serializedResult = JsonSerializer.Serialize(result);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return result;
        }
    }
}
