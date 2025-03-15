using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts;

/// <summary>
/// Command for retrieving an existing cart by its ID.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for retrieving a cart, 
/// including the cart ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="GetCartResponseDto"/>.
/// </remarks>
/// <param name="Id">The unique identifier of the cart to be retrieved.</param>
public record GetCartCommand(Guid Id) : IRequest<GetCartResponseDto>;
