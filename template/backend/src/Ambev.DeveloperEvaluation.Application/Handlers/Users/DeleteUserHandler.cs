using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users
{
    /// <summary>
    /// Handler for processing DeleteUserCommand requests.
    /// Deletes a user and invalidates the user cache and users list cache.
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisCacheService _cacheService;

        public DeleteUserHandler(IUserRepository userRepository, IRedisCacheService cacheService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
        }

        public async Task<DeleteUserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            // Invalida o cache do usuário individual e a listagem de usuários
            await _cacheService.RemoveAsync($"user_{request.Id}");
            await _cacheService.RemoveAsync("users_list");

            return new DeleteUserResponseDto { Success = true };
        }
    }
}
