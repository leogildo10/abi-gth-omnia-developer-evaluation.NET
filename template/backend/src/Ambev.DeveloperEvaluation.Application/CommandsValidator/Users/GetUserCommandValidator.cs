using Ambev.DeveloperEvaluation.Application.Commands.Users;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;

/// <summary>
/// Validator for <see cref="GetUserCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the user ID is correctly populated 
/// and follows the required rules for retrieving a user.
/// </remarks>
public class GetUserCommandValidator : AbstractValidator<GetUserCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetUserCommandValidator"/>.
    /// </summary>
    public GetUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");
    }
}




