using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales
{
    /// <summary>
    /// Handler for <see cref="ListSalesCommand"/>.
    /// Processes the request to list sales with pagination, using Redis as cache.
    /// </summary>
    public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResponseDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ListSalesResponseDto> Handle(ListSalesCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListSalesCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Define a chave do cache com base nos parâmetros de paginação
            var cacheKey = $"sales_list_page_{request.Page}_size_{request.Size}";

            // Tenta obter os dados do cache
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<ListSalesResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var query = _saleRepository.GetAllSales();
            var paginatedList = await PaginatedList<Sale>.CreateAsync(query, request.Page, request.Size);
            var salesDto = paginatedList.Select(s => _mapper.Map<GetSaleResponseDto>(s)).ToList();

            var result = new ListSalesResponseDto
            {
                Sales = salesDto,
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
