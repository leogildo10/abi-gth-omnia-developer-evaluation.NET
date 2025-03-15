using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts;

/// <summary>
/// Handler for <see cref="ListCartsCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to list carts with pagination by validating the input, 
/// querying the cart repository, mapping the results, and returning a paginated response.
/// </remarks>
public class ListCartsHandler : IRequestHandler<ListCartsCommand, ListCartsResponseDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListCartsHandler"/> class.
    /// </summary>
    /// <param name="cartRepository">The cart repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ListCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to list carts with pagination.
    /// </summary>
    /// <param name="request">The command containing pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the paginated list of carts.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    public async Task<ListCartsResponseDto> Handle(ListCartsCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListCartsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var query = _cartRepository.GetAllCarts();
        var paginatedList = await PaginatedList<Cart>.CreateAsync(query, request.Page, request.Size);

        var cartsDto = paginatedList.Select(c => _mapper.Map<GetCartResponseDto>(c)).ToList();

        return new ListCartsResponseDto
        {
            Carts = cartsDto,
            Page = paginatedList.CurrentPage,
            TotalCount = paginatedList.TotalCount
        };
    }
}
