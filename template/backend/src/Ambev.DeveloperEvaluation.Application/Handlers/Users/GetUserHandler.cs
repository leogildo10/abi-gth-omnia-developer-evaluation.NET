using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using Ambev.DeveloperEvaluation.Application.CommandsValidator.Users;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users
{
    /// <summary>
    /// Handler for processing GetUserCommand requests.
    /// Retrieves a user by ID, utilizing Redis cache.
    /// </summary>
    public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public GetUserHandler(IUserRepository userRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<GetUserResponseDto> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Define a chave de cache para o usuário individual
            var cacheKey = $"user_{request.Id}";
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<GetUserResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            var resultDto = _mapper.Map<GetUserResponseDto>(user);

            // Serializa e salva no cache com TTL de 5 minutos
            var serializedResult = JsonSerializer.Serialize(resultDto);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return resultDto;
        }
    }
}
