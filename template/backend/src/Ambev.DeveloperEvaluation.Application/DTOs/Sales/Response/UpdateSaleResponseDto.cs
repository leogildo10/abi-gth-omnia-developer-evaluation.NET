namespace Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;

/// <summary>
/// Represents the response after updating a sale.
/// </summary>
public class UpdateSaleResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
}
