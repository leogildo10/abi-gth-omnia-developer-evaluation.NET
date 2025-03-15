using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

/// <summary>
/// Validator for <see cref="UpdateSaleCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the sale ID, sale number, and other fields 
/// are correctly populated and follow the required rules for updating a sale.
/// </remarks>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateSaleCommandValidator"/>.
    /// </summary>
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Sale ID is required");
        When(x => x.SaleNumber != null, () =>
        {
            RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number cannot be empty");
        });
        // Other validations as needed, including for items
    }
}
