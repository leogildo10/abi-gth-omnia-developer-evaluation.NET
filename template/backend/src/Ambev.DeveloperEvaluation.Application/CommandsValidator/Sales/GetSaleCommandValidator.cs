using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

/// <summary>
/// Validator for <see cref="GetSaleCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the sale ID is correctly populated 
/// and follows the required rules for retrieving a sale.
/// </remarks>
public class GetSaleCommandValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetSaleCommandValidator"/>.
    /// </summary>
    public GetSaleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Sale ID is required");
    }
}
