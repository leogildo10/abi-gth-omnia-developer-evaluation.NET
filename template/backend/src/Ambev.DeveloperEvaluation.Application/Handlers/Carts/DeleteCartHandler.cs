using MediatR;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts;

/// <summary>
/// Handler for <see cref="DeleteCartCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to delete an existing cart by validating the input, 
/// and deleting the cart from the repository.
/// </remarks>
public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponseDto>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCartHandler"/> class.
    /// </summary>
    /// <param name="cartRepository">The cart repository instance.</param>
    public DeleteCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the request to delete an existing cart.
    /// </summary>
    /// <param name="request">The command containing cart ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response indicating whether the cart was successfully deleted.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the cart ID is invalid.</exception>
    public async Task<DeleteCartResponseDto> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new FluentValidation.ValidationException("Cart ID is required");

        await _cartRepository.DeleteAsync(request.Id, cancellationToken);
        return new DeleteCartResponseDto { Success = true };
    }
}
