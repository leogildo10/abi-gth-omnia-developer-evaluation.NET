using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products
{
    /// <summary>
    /// Handler for <see cref="CreateProductCommand"/>.
    /// Processes the request to create a new product and invalidates the product list cache.
    /// </summary>
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IRedisCacheService _cacheService;

        public CreateProductHandler(IMapper mapper, IProductRepository productRepository, IRedisCacheService cacheService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        public async Task<CreateProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var product = _mapper.Map<Product>(request);
            await _productRepository.CreateAsync(product, cancellationToken);

            // Invalida o cache da listagem de produtos, para que a próxima consulta seja atualizada.
            await _cacheService.RemoveAsync("products_list");

            return new CreateProductResponseDto { Id = product.Id, Title = product.Title };
        }
    }
}
