using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts;

/// <summary>
/// Handler for <see cref="UpdateCartCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to update an existing cart by validating the input, 
/// retrieving the cart from the repository, updating the cart's details, and saving the changes.
/// </remarks>
public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResponseDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCartHandler"/> class.
    /// </summary>
    /// <param name="cartRepository">The cart repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public UpdateCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to update an existing cart.
    /// </summary>
    /// <param name="request">The command containing cart update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the updated cart details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the cart is not found.</exception>
    public async Task<UpdateCartResponseDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

        if (request.UserId.HasValue)
            cart.UserId = request.UserId.Value;
        if (request.Date.HasValue)
            cart.Date = request.Date.Value;

        // Update items if provided (logic to update, add, or remove items)
        // Implement the necessary logic here.

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        return new UpdateCartResponseDto { Id = cart.Id };
    }
}
