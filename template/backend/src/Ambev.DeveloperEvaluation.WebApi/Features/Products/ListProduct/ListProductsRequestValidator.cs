using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    /// <summary>
    /// Validator for ListProductsRequest
    /// </summary>
    public class ListProductsRequestValidator : AbstractValidator<ListProductsRequest>
    {
        public ListProductsRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0");
            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0");
        }
    }
}
