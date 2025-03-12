using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser
{
    /// <summary>
    /// Validator for UpdateUserRequest
    /// </summary>
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of UpdateUserRequestValidator
        /// </summary>
        public UpdateUserRequestValidator()
        {
            When(x => x.Email != null, () => {
                RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format");
            });
            When(x => x.Username != null, () => {
                RuleFor(x => x.Username).Length(3, 50).WithMessage("Username must be between 3 and 50 characters");
            });
            When(x => x.Password != null, () => {
                RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters");
            });
            When(x => x.Phone != null, () => {
                RuleFor(x => x.Phone).Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone format");
            });
        }
    }
}

