using Xunit;
using Moq;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.Handlers.Users;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using AutoMapper;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class ListUsersHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ListUsersHandler _handler;

        public ListUsersHandlerTests()
        {
            _mapper = Substitute.For<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new ListUsersHandler(_userRepositoryMock.Object, _mapper, _cacheServiceMock.Object);
        }

        [Fact(DisplayName = "ListUsersHandler: Returns paginated list of users")]
        public async Task Handle_WithValidRequest_ShouldReturnPaginatedUsers()
        {
            var users = new[]
            {
                new User { Id = Guid.NewGuid(), Email = "a@example.com", Username = "UserA" },
                new User { Id = Guid.NewGuid(), Email = "b@example.com", Username = "UserB" }
            }.AsQueryable();
            _userRepositoryMock.Setup(r => r.GetAllUsers()).Returns(users);
            var command = new ListUsersCommand { Page = 1, Size = 10 };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Users.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }
    }
}
