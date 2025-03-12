using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers
{
    /// <summary>
    /// Validator for ListUsersRequest
    /// </summary>
    public class ListUsersRequestValidator : AbstractValidator<ListUsersRequest>
    {
        /// <summary>
        /// Initializes a new instance of ListUsersRequestValidator
        /// </summary>
        public ListUsersRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");
            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than zero.");
        }
    }
}

