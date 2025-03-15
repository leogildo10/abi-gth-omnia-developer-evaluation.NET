using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleRequest
    /// </summary>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            // Se "SaleNumber" for informado, verificar se não está vazio
            When(x => x.SaleNumber != null, () => {
                RuleFor(x => x.SaleNumber)
                    .NotEmpty().WithMessage("SaleNumber cannot be empty");
            });

            // Se "SaleDate" for informado, verifique se é válido (você pode colocar regras extras)
            // etc...
        }
    }
}
