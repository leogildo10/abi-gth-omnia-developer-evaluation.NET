using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Carts;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Carts;

/// <summary>
/// Handler for <see cref="CreateCartCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to create a new cart by validating the input, 
/// creating the cart entity, and saving the cart to the repository.
/// </remarks>
public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCartHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="cartRepository">The cart repository instance.</param>
    public CreateCartHandler(IMapper mapper, ICartRepository cartRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the request to create a new cart.
    /// </summary>
    /// <param name="request">The command containing cart details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the created cart details.</returns>
    /// <exception cref="FluentValidation.ValidationException">Thrown when the validation fails.</exception>
    public async Task<CreateCartResponseDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Date = request.Date,
            Items = request.Items.Select(i => new CartItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        await _cartRepository.CreateAsync(cart, cancellationToken);
        return new CreateCartResponseDto { Id = cart.Id };
    }
}
