using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleRequest
    /// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("SaleNumber is required");
            RuleFor(x => x.SaleDate).NotEmpty().WithMessage("SaleDate is required");
            RuleFor(x => x.Customer).NotEmpty().WithMessage("Customer is required");
            RuleFor(x => x.Branch).NotEmpty().WithMessage("Branch is required");

            RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}
