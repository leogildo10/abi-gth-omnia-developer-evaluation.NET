using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Validator for UpdateCartRequest.
    /// Validates the optional UserId, Date and the list of Items.
    /// </summary>
    public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartRequestValidator()
        {
            // Validação para UserId (se informado, não pode ser vazio)
            When(x => x.UserId.HasValue, () =>
            {
                RuleFor(x => x.UserId.Value)
                    .NotEmpty().WithMessage("UserId cannot be empty");
            });

            // Validação para Date (se informado, deve ser maior ou igual à data de hoje)
            When(x => x.Date.HasValue, () =>
            {
                RuleFor(x => x.Date.Value)
                    .GreaterThanOrEqualTo(DateTime.Today)
                    .WithMessage("Date cannot be in the past");
            });

            // Validação para a lista de Items (se houver itens)
            When(x => x.Items != null && x.Items.Any(), () =>
            {
                RuleForEach(x => x.Items).SetValidator(new UpdateCartItemRequestValidator());
            });
        }
    }

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
