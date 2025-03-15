using Ambev.DeveloperEvaluation.Application.DTOs.Carts;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts;

/// <summary>
/// Command for updating an existing cart.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a cart, 
/// including cart ID, user ID, date, and items. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="UpdateCartResponseDto"/>.
/// </remarks>
public class UpdateCartCommand : IRequest<UpdateCartResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the cart.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created or last updated.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the list of items included in the cart.
    /// </summary>
    public List<UpdateCartItemDto>? Items { get; set; }
}
