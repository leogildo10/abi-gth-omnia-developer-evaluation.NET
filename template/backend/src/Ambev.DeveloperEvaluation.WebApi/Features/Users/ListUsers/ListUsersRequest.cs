namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers
{
    /// <summary>
    /// Represents a request to list users with pagination
    /// </summary>
    public class ListUsersRequest
    {
        /// <summary>
        /// Gets or sets the page number for pagination
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the page size for pagination
        /// </summary>
        public int Size { get; set; } = 10;
    }
}

