using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;

/// <summary>
/// Interface for the cart repository.
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Creates a new cart asynchronously.
    /// </summary>
    /// <param name="cart">The cart entity to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created cart entity.</returns>
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a cart by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the cart.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cart entity if found; otherwise, null.</returns>
    Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all carts as an IQueryable.
    /// </summary>
    /// <returns>An IQueryable of carts.</returns>
    IQueryable<Cart> GetAllCarts();

    /// <summary>
    /// Updates an existing cart asynchronously.
    /// </summary>
    /// <param name="cart">The cart entity to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a cart by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
