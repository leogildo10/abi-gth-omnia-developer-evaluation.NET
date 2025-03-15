using Ambev.DeveloperEvaluation.Application.DTOs.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

/// <summary>
/// Validator for <see cref="CreateSaleItemDto"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the product ID, quantity, and unit price 
/// are correctly populated and follow the required rules for creating a sale item.
/// </remarks>
public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateSaleItemDtoValidator"/>.
    /// </summary>
    public CreateSaleItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is required");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
}
