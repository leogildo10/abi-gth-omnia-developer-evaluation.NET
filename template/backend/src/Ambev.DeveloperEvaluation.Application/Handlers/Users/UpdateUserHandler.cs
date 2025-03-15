using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users;

/// <summary>
/// Handler for <see cref="UpdateUserCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to update a user by validating the input, 
/// retrieving the user from the repository, updating the user's details, and saving the changes.
/// </remarks>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="passwordHasher">The password hasher instance.</param>
    public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the request to update a user.
    /// </summary>
    /// <param name="request">The command containing user update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the updated user details.</returns>
    /// <exception cref="ValidationException">Thrown when the validation fails.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    public async Task<UpdateUserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {request.Id} not found");

        if (!string.IsNullOrEmpty(request.Username))
            user.Username = request.Username;

        if (!string.IsNullOrEmpty(request.Email))
            user.Email = request.Email;

        if (!string.IsNullOrEmpty(request.Phone))
            user.Phone = request.Phone;

        if (request.Status.HasValue)
            user.Status = request.Status.Value;

        if (request.Role.HasValue)
            user.Role = request.Role.Value;

        if (!string.IsNullOrEmpty(request.Password))
            user.Password = _passwordHasher.HashPassword(request.Password);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return _mapper.Map<UpdateUserResponseDto>(user);
    }
}
