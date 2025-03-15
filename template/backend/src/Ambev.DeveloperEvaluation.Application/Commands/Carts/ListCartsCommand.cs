using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts;

/// <summary>
/// Command for listing carts with pagination.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for listing carts, 
/// including the page number and page size for pagination. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListCartsResponseDto"/>.
/// </remarks>
public class ListCartsCommand : IRequest<ListCartsResponseDto>
{
    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the page size for pagination.
    /// </summary>
    public int Size { get; set; }
}
