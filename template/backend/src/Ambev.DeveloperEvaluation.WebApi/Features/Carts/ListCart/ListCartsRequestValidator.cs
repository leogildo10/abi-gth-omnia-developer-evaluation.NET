using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    /// <summary>
    /// Validator for ListCartsRequest
    /// </summary>
    public class ListCartsRequestValidator : AbstractValidator<ListCartsRequest>
    {
        public ListCartsRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0");
            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0");
        }
    }
}
