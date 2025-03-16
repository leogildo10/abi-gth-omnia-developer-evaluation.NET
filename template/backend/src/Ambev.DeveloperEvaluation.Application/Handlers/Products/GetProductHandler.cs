using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products
{
    /// <summary>
    /// Handler for <see cref="GetProductCommand"/>.
    /// Processes the request to retrieve a product, utilizing Redis cache.
    /// </summary>
    public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public GetProductHandler(IProductRepository productRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<GetProductResponseDto> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetProductCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache para o produto individual
            var cacheKey = $"product_{request.Id}";
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<GetProductResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            var resultDto = _mapper.Map<GetProductResponseDto>(product);

            // Salva no cache com TTL de 5 minutos
            var serializedResult = JsonSerializer.Serialize(resultDto);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return resultDto;
        }
    }
}
