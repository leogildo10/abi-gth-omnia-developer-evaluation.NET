using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts;

/// <summary>
/// Handler for <see cref="GetCartCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to retrieve an existing cart by validating the input, 
/// retrieving the cart from the repository, and mapping the cart to the response DTO.
/// </remarks>
public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResponseDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartHandler"/> class.
    /// </summary>
    /// <param name="cartRepository">The cart repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve an existing cart.
    /// </summary>
    /// <param name="request">The command containing cart ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the cart details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the cart is not found.</exception>
    public async Task<GetCartResponseDto> Handle(GetCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetCartCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);

        if (cart == null)
            throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

        return _mapper.Map<GetCartResponseDto>(cart);
    }
}
