using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users
{
    /// <summary>
    /// Handler for processing ListUsersCommand requests.
    /// Retrieves a paginated list of users, utilizing Redis cache.
    /// </summary>
    public class ListUsersHandler : IRequestHandler<ListUsersCommand, ListUsersResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public ListUsersHandler(IUserRepository userRepository, IMapper mapper, IRedisCacheService cacheService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ListUsersResponseDto> Handle(ListUsersCommand request, CancellationToken cancellationToken)
        {
            // Define uma chave de cache baseada em parâmetros de paginação.
            //var cacheKey = $"users_list_page_{request.Page}_size_{request.Size}";
            var cacheKey = $"users_list";
            var cachedData = await _cacheService.GetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var resultFromCache = JsonSerializer.Deserialize<ListUsersResponseDto>(cachedData);
                if (resultFromCache != null)
                    return resultFromCache;
            }

            var query = _userRepository.GetAllUsers();
            var paginatedUsers = await PaginatedList<User>.CreateAsync(query, request.Page, request.Size);

            // Mapeia os usuários para o DTO de resposta.
            var usersDto = paginatedUsers.Select(u => _mapper.Map<User>(u)).ToList();

            var result = new ListUsersResponseDto
            {
                Users = usersDto,
                CurrentPage = paginatedUsers.CurrentPage,
                TotalPages = paginatedUsers.TotalPages,
                TotalCount = paginatedUsers.TotalCount
            };

            // Serializa e salva o resultado no cache com TTL de 5 minutos.
            var serializedResult = JsonSerializer.Serialize(result);
            await _cacheService.SetAsync(cacheKey, serializedResult);

            return result;
        }
    }
}
