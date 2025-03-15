using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Products;

/// <summary>
/// Command for retrieving an existing product by its ID.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for retrieving a product, 
/// including the product ID. It implements <see cref="IRequest{TResponse}"/> 
/// to initiate the request that returns a <see cref="GetProductResponseDto"/>.
/// </remarks>
/// <param name="Id">The unique identifier of the product to be retrieved.</param>
public record GetProductCommand(Guid Id) : IRequest<GetProductResponseDto>;
