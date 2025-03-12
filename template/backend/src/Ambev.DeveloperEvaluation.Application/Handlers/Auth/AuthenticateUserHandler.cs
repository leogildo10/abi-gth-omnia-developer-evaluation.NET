using Ambev.DeveloperEvaluation.Application.Commands.Auth;
using Ambev.DeveloperEvaluation.Application.DTOs.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Auth;

/// <summary>
/// Handler for <see cref="AuthenticateUserCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the authentication request by verifying the user's credentials,
/// checking if the user is active, and generating a JWT token if authentication is successful.
/// </remarks>
public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="passwordHasher">The password hasher instance.</param>
    /// <param name="jwtTokenGenerator">The JWT token generator instance.</param>
    public AuthenticateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <summary>
    /// Handles the authentication request.
    /// </summary>
    /// <param name="request">The authentication command containing user credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The authentication response containing the JWT token and user details.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the credentials are invalid or the user is not active.</exception>
    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var activeUserSpec = new ActiveUserSpecification();
        if (!activeUserSpec.IsSatisfiedBy(user))
        {
            throw new UnauthorizedAccessException("User is not active");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticateUserResponse
        {
            Token = token,
            Email = user.Email,
            Name = user.Username,
            Role = user.Role.ToString()
        };
    }
}
