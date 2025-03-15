using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

/// <summary>
/// Validator for <see cref="ListCartsCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the page number and page size are correctly populated 
/// and follow the required rules for listing carts with pagination.
/// </remarks>
public class ListCartsCommandValidator : AbstractValidator<ListCartsCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ListCartsCommandValidator"/>.
    /// </summary>
    public ListCartsCommandValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0");
    }
}
