using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Validator for ListSalesRequest
    /// </summary>
    public class ListSalesRequestValidator : AbstractValidator<ListSalesRequest>
    {
        public ListSalesRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");
            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than zero.");
        }
    }
}
