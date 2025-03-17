using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Validator for UpdateCartItemRequest.
    /// Ensures that each cart item has the required fields.
    /// </summary>
    public class UpdateCartItemRequestValidator : AbstractValidator<UpdateCartItemRequest>
    {
        public UpdateCartItemRequestValidator()
        {
            // Produto é obrigatório para cada item
            RuleFor(x => x.ProductId)
                .NotNull().WithMessage("ProductId is required for each item")
                .NotEmpty().When(x => x.ProductId.HasValue)
                .WithMessage("ProductId cannot be empty");

            // Quantidade é obrigatória e deve ser maior que 0
            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Quantity is required for each item")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .When(x => x.Quantity.HasValue);
        }
    }
}
