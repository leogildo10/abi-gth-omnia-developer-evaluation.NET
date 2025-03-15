using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

/// <summary>
/// Validator for <see cref="CreateSaleCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the sale number, sale date, customer, branch, and items 
/// are correctly populated and follow the required rules for creating a sale.
/// </remarks>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateSaleCommandValidator"/>.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number is required");
        RuleFor(x => x.SaleDate).NotEmpty().WithMessage("Sale date is required");
        RuleFor(x => x.Customer).NotEmpty().WithMessage("Customer is required");
        RuleFor(x => x.Branch).NotEmpty().WithMessage("Branch is required");
        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemDtoValidator());
    }
}
