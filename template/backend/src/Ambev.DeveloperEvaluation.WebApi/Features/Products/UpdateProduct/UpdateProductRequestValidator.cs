using FluentValidation;
using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    /// <summary>
    /// Validator for UpdateProductRequest.
    /// Validates all optional fields if they are provided.
    /// </summary>
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            // Validate Title: se for informado, não pode ser vazio e deve ter entre 3 e 100 caracteres.
            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title cannot be empty")
                    .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                    .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
            });

            // Validate Price: se for informado, deve ser maior que 0.
            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price.Value)
                    .GreaterThan(0).WithMessage("Price must be greater than 0");
            });

            // Validate Description: se for informado, não pode ser vazio e deve ter entre 10 e 1000 caracteres.
            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description cannot be empty")
                    .MinimumLength(10).WithMessage("Description must be at least 10 characters")
                    .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
            });

            // Validate Category: se for informado, não pode ser vazio e deve ter no máximo 50 caracteres.
            When(x => x.Category != null, () =>
            {
                RuleFor(x => x.Category)
                    .NotEmpty().WithMessage("Category cannot be empty")
                    .MaximumLength(50).WithMessage("Category must not exceed 50 characters");
            });

            // Validate ImageUrl: se for informado, não pode ser vazio e deve ser uma URL válida.
            When(x => x.ImageUrl != null, () =>
            {
                RuleFor(x => x.ImageUrl)
                    .NotEmpty().WithMessage("ImageUrl cannot be empty")
                    .Must(BeAValidUrl).WithMessage("ImageUrl must be a valid URL");
            });
        }

        /// <summary>
        /// Validates if the provided string is a well-formed absolute URL.
        /// </summary>
        /// <param name="imageUrl">The image URL string.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool BeAValidUrl(string imageUrl)
        {
            return Uri.TryCreate(imageUrl, UriKind.Absolute, out _);
        }
    }
}
