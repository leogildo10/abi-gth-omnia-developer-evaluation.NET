using Ambev.DeveloperEvaluation.Application.DTOs.Carts;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts;

/// <summary>
/// Command for creating a new cart.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a cart, 
/// including user ID, date, and items. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="CreateCartResponseDto"/>.
/// </remarks>
public class CreateCartCommand : IRequest<CreateCartResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the cart.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the list of items included in the cart.
    /// </summary>
    public List<CreateCartItemDto> Items { get; set; } = new();
}
