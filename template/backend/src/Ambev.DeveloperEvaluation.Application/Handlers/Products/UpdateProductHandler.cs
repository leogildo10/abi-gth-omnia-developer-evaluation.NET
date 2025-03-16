using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products
{
    /// <summary>
    /// Handler for <see cref="UpdateProductCommand"/>.
    /// Processes the request to update an existing product and invalidates related cache entries.
    /// </summary>
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IRedisCacheService _cacheService;

        public UpdateProductHandler(IMapper mapper, IProductRepository productRepository, IRedisCacheService cacheService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        public async Task<UpdateProductResponseDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            if (!string.IsNullOrEmpty(request.Title))
                product.Title = request.Title;
            if (request.Price.HasValue)
                product.Price = request.Price.Value;
            if (!string.IsNullOrEmpty(request.Description))
                product.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Category))
                product.Category = request.Category;
            if (!string.IsNullOrEmpty(request.ImageUrl))
                product.ImageUrl = request.ImageUrl;

            await _productRepository.UpdateAsync(product, cancellationToken);

            // Invalida o cache do produto individual e da listagem geral.
            await _cacheService.RemoveAsync($"product_{request.Id}");
            await _cacheService.RemoveAsync("products_list");

            return new UpdateProductResponseDto { Id = product.Id, Title = product.Title };
        }
    }
}
