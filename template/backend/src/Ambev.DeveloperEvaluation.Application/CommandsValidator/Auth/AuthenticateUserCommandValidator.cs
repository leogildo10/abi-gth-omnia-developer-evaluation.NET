using Ambev.DeveloperEvaluation.Application.Commands.Auth;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Auth;

/// <summary>
/// Validator for <see cref="AuthenticateUserCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the email and password fields are correctly populated 
/// and follow the required rules for authentication.
/// </remarks>
public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AuthenticateUserCommandValidator"/>.
    /// </summary>
    public AuthenticateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
    }
}


