using Xunit;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "UsersController: POST /api/users returns 201")]
        public async Task PostUser_ShouldReturn201()
        {
            var requestBody = new
            {
                username = "JohnDoe",
                password = "Secret123",
                phone = "+11234567890",
                email = "john.doe@example.com",
                status = 1,
                role = 1
            };
            var response = await _client.PostAsJsonAsync("/api/users", requestBody);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "UsersController: GET /api/users returns 200")]
        public async Task GetUsers_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/users");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
