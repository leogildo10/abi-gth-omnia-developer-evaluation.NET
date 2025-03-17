using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Validator for CreateCartRequest
    /// </summary>
    public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");
            RuleForEach(x => x.Items).SetValidator(new CreateCartItemRequestValidator());
        }
    }
}
