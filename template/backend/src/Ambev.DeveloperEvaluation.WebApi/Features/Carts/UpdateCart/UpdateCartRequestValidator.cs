using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Validator for UpdateCartRequest
    /// </summary>
    public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartRequestValidator()
        {
            // Exemplo: Se UserId for informado, não pode ser vazio
            When(x => x.UserId.HasValue, () =>
            {
                RuleFor(x => x.UserId.Value)
                    .NotEmpty().WithMessage("UserId cannot be empty");
            });
            // etc.
        }
    }
}
