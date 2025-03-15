using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

/// <summary>
/// Validator for <see cref="ListSalesCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the page number and page size are correctly populated 
/// and follow the required rules for listing sales with pagination.
/// </remarks>
public class ListSalesCommandValidator : AbstractValidator<ListSalesCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ListSalesCommandValidator"/>.
    /// </summary>
    public ListSalesCommandValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0");
    }
}
