using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

/// <summary>
/// Command for deleting an existing sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for deleting a sale, 
/// including the sale ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="DeleteSaleResponseDto"/>.
/// </remarks>
public class DeleteSaleCommand : IRequest<DeleteSaleResponseDto>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to be deleted.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleCommand"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be deleted.</param>
    public DeleteSaleCommand(Guid id) { Id = id; }
}
