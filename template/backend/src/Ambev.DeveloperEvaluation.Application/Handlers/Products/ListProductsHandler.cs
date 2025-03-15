using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;
using Ambev.DeveloperEvaluation.Application.Commands.Products;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Products;

/// <summary>
/// Handler for <see cref="ListProductsCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to list products with pagination by validating the input, 
/// querying the product repository, mapping the results, and returning a paginated response.
/// </remarks>
public class ListProductsHandler : IRequestHandler<ListProductsCommand, ListProductsResponseDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListProductsHandler"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ListProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to list products with pagination.
    /// </summary>
    /// <param name="request">The command containing pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the paginated list of products.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    public async Task<ListProductsResponseDto> Handle(ListProductsCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListProductsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var query = _productRepository.GetAllProducts();
        var paginatedList = await PaginatedList<Product>.CreateAsync(query, request.Page, request.Size);

        var productsDto = paginatedList.Select(p => _mapper.Map<GetProductResponseDto>(p)).ToList();

        return new ListProductsResponseDto
        {
            Products = productsDto,
            Page = paginatedList.CurrentPage,
            TotalCount = paginatedList.TotalCount
        };
    }
}

