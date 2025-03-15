using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

/// <summary>
/// Command for listing sales with pagination.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for listing sales, 
/// including the page number and page size for pagination. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListSalesResponseDto"/>.
/// </remarks>
public class ListSalesCommand : IRequest<ListSalesResponseDto>
{
    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the page size for pagination.
    /// </summary>
    public int Size { get; set; }
}
