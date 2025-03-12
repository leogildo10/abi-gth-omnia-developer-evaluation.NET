using Ambev.DeveloperEvaluation.Application.Commands.Users;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;

/// <summary>
/// Validator for <see cref="UpdateUserCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the user ID, username, email, password, phone number, status, and role 
/// are correctly populated and follow the required rules for updating a user.
/// </remarks>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateUserCommandValidator"/>.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone format");
        });

        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status)
                .NotEqual(UserStatus.Unknown).WithMessage("Invalid user status");
        });

        When(x => x.Role.HasValue, () =>
        {
            RuleFor(x => x.Role)
                .NotEqual(UserRole.None).WithMessage("Invalid user role");
        });
    }
}
