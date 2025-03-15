using Ambev.DeveloperEvaluation.Application.Commands.Products;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;

/// <summary>
/// Validator for <see cref="ListProductsCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the page number and page size are correctly populated 
/// and follow the required rules for listing products with pagination.
/// </remarks>
public class ListProductsCommandValidator : AbstractValidator<ListProductsCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ListProductsCommandValidator"/>.
    /// </summary>
    public ListProductsCommandValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0");
    }
}
