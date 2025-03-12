namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser
{
    /// <summary>
    /// Represents a request to update an existing user
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the username of the user
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email of the user
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the status of the user
        /// </summary>
        public Domain.Enums.UserStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the role of the user
        /// </summary>
        public Domain.Enums.UserRole? Role { get; set; }
    }
}

