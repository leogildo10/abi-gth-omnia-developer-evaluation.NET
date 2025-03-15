using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products;

/// <summary>
/// Handler for <see cref="CreateProductCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to create a new product by validating the input, 
/// mapping the command to the product entity, and saving the product to the repository.
/// </remarks>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="productRepository">The product repository instance.</param>
    public CreateProductHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the request to create a new product.
    /// </summary>
    /// <param name="request">The command containing product details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the created product details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    public async Task<CreateProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var product = _mapper.Map<Product>(request);
        await _productRepository.CreateAsync(product, cancellationToken);

        return new CreateProductResponseDto { Id = product.Id, Title = product.Title };
    }
}
