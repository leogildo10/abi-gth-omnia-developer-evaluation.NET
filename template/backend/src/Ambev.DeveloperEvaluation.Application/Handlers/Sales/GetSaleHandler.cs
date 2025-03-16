using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="GetSaleCommand"/>.
    /// Processes the request to retrieve an existing sale, utilizing Redis cache.
    /// </summary>
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResponseDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<GetSaleResponseDto> Handle(GetSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache para o registro individual da venda.
            var cacheKey = $"sale_{request.Id}";
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<GetSaleResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            var resultDto = _mapper.Map<GetSaleResponseDto>(sale);

            // Serializa e salva no cache com TTL de 5 minutos.
            var serializedResult = JsonSerializer.Serialize(resultDto);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return resultDto;
        }
    }
}
