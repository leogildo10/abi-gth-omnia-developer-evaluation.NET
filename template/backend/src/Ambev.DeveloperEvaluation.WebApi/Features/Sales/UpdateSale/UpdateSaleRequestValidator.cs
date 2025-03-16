using FluentValidation;
using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleRequest.
    /// Validates optional fields in the request if they are provided.
    /// </summary>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            // Validate SaleNumber if provided
            When(x => x.SaleNumber != null, () =>
            {
                RuleFor(x => x.SaleNumber)
                    .NotEmpty().WithMessage("SaleNumber cannot be empty")
                    .MinimumLength(3).WithMessage("SaleNumber must be at least 3 characters")
                    .MaximumLength(50).WithMessage("SaleNumber must not exceed 50 characters");
            });

            // Validate SaleDate if provided
            When(x => x.SaleDate.HasValue, () =>
            {
                RuleFor(x => x.SaleDate.Value)
                    .NotEqual(DateTime.MinValue).WithMessage("SaleDate must be a valid date");
                // Se necessário, descomente a linha abaixo para não permitir datas futuras:
                //.LessThanOrEqualTo(DateTime.Today).WithMessage("SaleDate cannot be in the future");
            });

            // Validate Customer if provided
            When(x => x.Customer != null, () =>
            {
                RuleFor(x => x.Customer)
                    .NotEmpty().WithMessage("Customer cannot be empty")
                    .MinimumLength(3).WithMessage("Customer must be at least 3 characters")
                    .MaximumLength(100).WithMessage("Customer must not exceed 100 characters");
            });

            // Validate Branch if provided
            When(x => x.Branch != null, () =>
            {
                RuleFor(x => x.Branch)
                    .NotEmpty().WithMessage("Branch cannot be empty")
                    .MinimumLength(2).WithMessage("Branch must be at least 2 characters")
                    .MaximumLength(50).WithMessage("Branch must not exceed 50 characters");
            });

            // Validate Items if provided and not empty
            When(x => x.Items != null && x.Items.Count > 0, () =>
            {
                RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemRequestValidator());
            });
        }
    }

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
