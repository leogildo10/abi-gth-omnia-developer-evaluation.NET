using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Handlers.Users;

/// <summary>
/// Handler for <see cref="ListUsersCommand"/>.
/// </summary>
/// <remarks>
/// This handler processes the request to list users with pagination by querying the user repository,
/// mapping the results, and returning a paginated response.
/// </remarks>
public class ListUsersHandler : IRequestHandler<ListUsersCommand, ListUsersResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListUsersHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ListUsersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to list users with pagination.
    /// </summary>
    /// <param name="request">The command containing pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the paginated list of users.</returns>
    public async Task<ListUsersResponse> Handle(ListUsersCommand request, CancellationToken cancellationToken)
    {
        var query = _userRepository.GetAllUsers();
        var paginatedUsers = await PaginatedList<User>.CreateAsync(query, request.Page, request.Size);

        var usersDto = paginatedUsers.Select(u => _mapper.Map<User>(u)).ToList();

        return new ListUsersResponse
        {
            Users = usersDto,
            CurrentPage = paginatedUsers.CurrentPage,
            TotalPages = paginatedUsers.TotalPages,
            TotalCount = paginatedUsers.TotalCount
        };
    }
}
