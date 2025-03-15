using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

/// <summary>
/// Validator for <see cref="UpdateCartCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the cart ID is correctly populated 
/// and follows the required rules for updating a cart.
/// </remarks>
public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateCartCommandValidator"/>.
    /// </summary>
    public UpdateCartCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cart ID is required");
        // Additional validations can be included as needed.
    }
}
