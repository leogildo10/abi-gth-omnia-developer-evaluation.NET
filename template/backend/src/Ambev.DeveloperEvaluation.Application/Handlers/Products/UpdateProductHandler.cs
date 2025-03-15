using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products;

/// <summary>
/// Handler for <see cref="UpdateProductCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to update an existing product by validating the input, 
/// retrieving the product from the repository, updating the product's details, and saving the changes.
/// </remarks>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="productRepository">The product repository instance.</param>
    public UpdateProductHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the request to update an existing product.
    /// </summary>
    /// <param name="request">The command containing product update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the updated product details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the product is not found.</exception>
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
        return new UpdateProductResponseDto { Id = product.Id, Title = product.Title };
    }
}
