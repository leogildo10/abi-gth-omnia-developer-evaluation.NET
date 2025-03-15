using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Users;

/// <summary>
/// Command for listing users with pagination.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for listing users, 
/// including the page number and page size for pagination. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListUsersResponseDto"/>.
/// </remarks>
public record ListUsersCommand : IRequest<ListUsersResponseDto>
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


