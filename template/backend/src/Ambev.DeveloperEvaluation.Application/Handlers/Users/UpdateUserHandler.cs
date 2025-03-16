using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users
{
    /// <summary>
    /// Handler for processing UpdateUserCommand requests.
    /// Updates an existing user and invalidates the user and users list caches.
    /// </summary>
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRedisCacheService _cacheService;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IRedisCacheService cacheService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _cacheService = cacheService;
        }

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

            // Invalida o cache do usuário individual e a listagem de usuários.
            await _cacheService.RemoveAsync($"user_{request.Id}");
            await _cacheService.RemoveAsync("users_list");

            return _mapper.Map<UpdateUserResponseDto>(user);
        }
    }
}
