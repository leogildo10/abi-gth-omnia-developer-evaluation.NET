using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    /// <summary>
    /// Validator for UpdateProductRequest
    /// </summary>
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            // Se Title for informado, verificar se não está vazio
            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
            });

            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price.Value).GreaterThan(0).WithMessage("Price must be greater than 0");
            });

            // etc. para Description, Category, ImageUrl
        }
    }
}
