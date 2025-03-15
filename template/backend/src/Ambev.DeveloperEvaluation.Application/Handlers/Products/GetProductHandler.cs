using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Application.Commands.Products;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products;

/// <summary>
/// Handler for <see cref="GetProductCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to retrieve an existing product by validating the input, 
/// retrieving the product from the repository, and mapping the product to the response DTO.
/// </remarks>
public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResponseDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandler"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve an existing product.
    /// </summary>
    /// <param name="request">The command containing product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the product details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the product is not found.</exception>
    public async Task<GetProductResponseDto> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetProductCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        return _mapper.Map<GetProductResponseDto>(product);
    }
}
