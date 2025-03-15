using Ambev.DeveloperEvaluation.Application.Commands.Products;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Products;

/// <summary>
/// Validator for <see cref="CreateProductCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the title, price, description, category, and image URL 
/// are correctly populated and follow the required rules for creating a product.
/// </remarks>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateProductCommandValidator"/>.
    /// </summary>
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required");
    }
}
