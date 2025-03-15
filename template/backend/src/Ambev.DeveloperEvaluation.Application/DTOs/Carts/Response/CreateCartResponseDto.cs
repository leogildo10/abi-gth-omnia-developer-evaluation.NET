namespace Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;

/// <summary>
/// Represents the response after creating a cart.
/// </summary>
public class CreateCartResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart.
    /// </summary>
    public Guid Id { get; set; }
}
