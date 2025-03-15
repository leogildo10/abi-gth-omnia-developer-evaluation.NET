using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using MediatR;

/// <summary>
/// Command for retrieving an existing sale by its ID.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for retrieving a sale, 
/// including the sale ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="GetSaleResponseDto"/>.
/// </remarks>
/// <param name="Id">The unique identifier of the sale to be retrieved.</param>
namespace Ambev.DeveloperEvaluation.Application.Commands.Sales
{
    public record GetSaleCommand(Guid Id) : IRequest<GetSaleResponseDto>;
}
