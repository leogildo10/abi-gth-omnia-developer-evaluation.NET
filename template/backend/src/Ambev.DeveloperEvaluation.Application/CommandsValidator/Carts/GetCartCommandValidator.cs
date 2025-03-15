using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

/// <summary>
/// Validator for <see cref="GetCartCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the cart ID is correctly populated 
/// and follows the required rules for retrieving a cart.
/// </remarks>
public class GetCartCommandValidator : AbstractValidator<GetCartCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetCartCommandValidator"/>.
    /// </summary>
    public GetCartCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cart ID is required");
    }
}
