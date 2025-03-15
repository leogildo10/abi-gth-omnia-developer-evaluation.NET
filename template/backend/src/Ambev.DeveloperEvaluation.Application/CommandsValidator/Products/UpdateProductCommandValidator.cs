using Ambev.DeveloperEvaluation.Application.Commands.Products;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;

/// <summary>
/// Validator for <see cref="UpdateProductCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the product ID, title, price, and other fields 
/// are correctly populated and follow the required rules for updating a product.
/// </remarks>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateProductCommandValidator"/>.
    /// </summary>
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
        });
        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price.Value).GreaterThan(0).WithMessage("Price must be greater than 0");
        });
        // Other validations as needed.
    }
}
