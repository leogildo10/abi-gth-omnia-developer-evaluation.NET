using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts;

/// <summary>
/// Command for deleting an existing cart.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for deleting a cart, 
/// including the cart ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="DeleteCartResponseDto"/>.
/// </remarks>
public class DeleteCartCommand : IRequest<DeleteCartResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart to be deleted.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCartCommand"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the cart to be deleted.</param>
    public DeleteCartCommand(Guid id) { Id = id; }
}
