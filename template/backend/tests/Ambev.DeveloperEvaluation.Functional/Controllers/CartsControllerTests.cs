using Xunit;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class CartsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CartsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "CartsController: POST /api/carts returns 200/201")]
        public async Task PostCart_ShouldReturn200Or201()
        {
            var requestBody = new
            {
                userId = "5d10fe38-48dd-4c7e-80a3-d41957638534",
                date = "2025-03-30T12:00:00Z",
                items = new[]
                {
                    new { productId = "0195a090-a2f3-71bc-bfb2-44b4d6e759a3", quantity = 2 }
                }
            };
            var response = await _client.PostAsJsonAsync("/api/carts", requestBody);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "CartsController: GET /api/carts returns 200")]
        public async Task GetCarts_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/carts");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
