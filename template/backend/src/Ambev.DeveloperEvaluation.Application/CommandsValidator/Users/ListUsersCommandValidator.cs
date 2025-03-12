using Ambev.DeveloperEvaluation.Application.Commands.Users;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;

/// <summary>
/// Validator for <see cref="ListUsersCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the page number and page size are correctly populated 
/// and follow the required rules for listing users with pagination.
/// </remarks>
public class ListUsersCommandValidator : AbstractValidator<ListUsersCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ListUsersCommandValidator"/>.
    /// </summary>
    public ListUsersCommandValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");
        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Size must be greater than 0.");
    }
}




