using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

/// <summary>
/// Validator for <see cref="CreateCartCommand"/>.
/// </summary>
/// <remarks>
/// This validator ensures that the user ID, date, and items 
/// are correctly populated and follow the required rules for creating a cart.
/// </remarks>
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateCartCommandValidator"/>.
    /// </summary>
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required");
        RuleForEach(x => x.Items).SetValidator(new CreateCartItemDtoValidator());
    }
}
