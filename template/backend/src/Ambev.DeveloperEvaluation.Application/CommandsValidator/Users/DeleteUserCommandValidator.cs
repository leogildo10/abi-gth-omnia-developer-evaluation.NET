using Ambev.DeveloperEvaluation.Application.Commands.Users;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;

/// <summary>
/// Validator for <see cref="DeleteUserCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the user ID is correctly populated 
/// and follows the required rules for deleting a user.
/// </remarks>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="DeleteUserCommandValidator"/>.
    /// </summary>
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");
    }
}



