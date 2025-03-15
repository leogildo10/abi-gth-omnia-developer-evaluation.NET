using Ambev.DeveloperEvaluation.Application.Commands.Products;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;

/// <summary>
/// Validator for <see cref="GetProductCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the product ID is correctly populated 
/// and follows the required rules for retrieving a product.
/// </remarks>
public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetProductCommandValidator"/>.
    /// </summary>
    public GetProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}
