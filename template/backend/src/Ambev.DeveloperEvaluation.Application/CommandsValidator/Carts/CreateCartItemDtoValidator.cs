using Ambev.DeveloperEvaluation.Application.DTOs.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

/// <summary>
/// Validator for <see cref="CreateCartItemDto"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the product ID and quantity 
/// are correctly populated and follow the required rules for creating a cart item.
/// </remarks>
public class CreateCartItemDtoValidator : AbstractValidator<CreateCartItemDto>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateCartItemDtoValidator"/>.
    /// </summary>
    public CreateCartItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
