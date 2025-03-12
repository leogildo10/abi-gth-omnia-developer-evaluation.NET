using Ambev.DeveloperEvaluation.Application.DTOs.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Users;

/// <summary>
/// Command for updating an existing user.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a user, 
/// including the user ID, username, email, password, phone number, status, and role. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UpdateUserResponse"/>.
/// </remarks>
public class UpdateUserCommand : IRequest<UpdateUserResponse>
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    public UserStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public UserRole? Role { get; set; }
}


