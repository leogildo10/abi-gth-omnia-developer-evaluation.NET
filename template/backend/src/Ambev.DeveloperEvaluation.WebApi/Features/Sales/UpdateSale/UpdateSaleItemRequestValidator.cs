using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleItemRequest.
    /// Validates each sale item provided in the update request.
    /// </summary>
    public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
    {
        public UpdateSaleItemRequestValidator()
        {
            // Validate ProductId if provided
            When(x => x.ProductId.HasValue, () =>
            {
                RuleFor(x => x.ProductId.Value)
                    .NotEmpty().WithMessage("ProductId cannot be empty");
            });

            // Validate Quantity if provided
            When(x => x.Quantity.HasValue, () =>
            {
                RuleFor(x => x.Quantity.Value)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                    .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");
            });

            // Validate UnitPrice if provided
            When(x => x.UnitPrice.HasValue, () =>
            {
                RuleFor(x => x.UnitPrice.Value)
                    .GreaterThan(0).WithMessage("UnitPrice must be greater than 0");
            });
        }
    }
}
