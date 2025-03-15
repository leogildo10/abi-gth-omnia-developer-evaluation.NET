using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Sales;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Sales;

/// <summary>
/// Handler for <see cref="ListSalesCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to list sales with pagination by validating the input, 
/// querying the sale repository, mapping the results, and returning a paginated response.
/// </remarks>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResponseDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListSalesHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The sale repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to list sales with pagination.
    /// </summary>
    /// <param name="request">The command containing pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the paginated list of sales.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    public async Task<ListSalesResponseDto> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListSalesCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var query = _saleRepository.GetAllSales();
        var paginatedList = await PaginatedList<Sale>.CreateAsync(query, request.Page, request.Size);

        var salesDto = paginatedList.Select(s => _mapper.Map<GetSaleResponseDto>(s)).ToList();

        return new ListSalesResponseDto
        {
            Sales = salesDto,
            Page = paginatedList.CurrentPage,
            TotalCount = paginatedList.TotalCount
        };
    }
}
