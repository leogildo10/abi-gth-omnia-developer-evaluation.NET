using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales;

/// <summary>
/// Handler for <see cref="GetSaleCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to retrieve an existing sale by validating the input, 
/// retrieving the sale from the repository, and mapping the sale to the response DTO.
/// </remarks>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResponseDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The sale repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve an existing sale.
    /// </summary>
    /// <param name="request">The command containing sale ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the sale details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the sale is not found.</exception>
    public async Task<GetSaleResponseDto> Handle(GetSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        return _mapper.Map<GetSaleResponseDto>(sale);
    }
}
