using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products
{
    /// <summary>
    /// Handler for <see cref="DeleteProductCommand"/>.
    /// Processes the deletion of a product and invalidates related cache entries.
    /// </summary>
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisCacheService _cacheService;

        public DeleteProductHandler(IProductRepository productRepository, IRedisCacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        public async Task<DeleteProductResponseDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new FluentValidation.ValidationException("Product ID is required");

            var success = await _productRepository.DeleteAsync(request.Id, cancellationToken);

            if (!success)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            // Invalida o cache do produto individual e da listagem.
            await _cacheService.RemoveAsync($"product_{request.Id}");
            await _cacheService.RemoveAsync("products_list");

            return new DeleteProductResponseDto { Success = true };
        }
    }
}
