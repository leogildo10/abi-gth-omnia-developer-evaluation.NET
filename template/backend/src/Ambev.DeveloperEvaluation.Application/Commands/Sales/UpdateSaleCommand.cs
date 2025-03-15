using Ambev.DeveloperEvaluation.Application.DTOs.Sales;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a sale, 
/// including sale ID, sale number, sale date, customer, branch, and items. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UpdateSaleResponseDto"/>.
/// </remarks>
public class UpdateSaleCommand : IRequest<UpdateSaleResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string? SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer associated with the sale.
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Gets or sets the branch where the sale occurred.
    /// </summary>
    public string? Branch { get; set; }

    /// <summary>
    /// Gets or sets the list of items included in the sale.
    /// </summary>
    public List<UpdateSaleItemDto>? Items { get; set; }
}
