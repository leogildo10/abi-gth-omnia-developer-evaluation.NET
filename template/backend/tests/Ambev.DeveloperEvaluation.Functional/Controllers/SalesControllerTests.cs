using Xunit;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class SalesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "SalesController: POST /api/sales returns 200/201")]
        public async Task PostSale_ShouldReturn200Or201()
        {
            var requestBody = new
            {
                saleNumber = "S-001",
                saleDate = "2025-03-17T12:00:00Z",
                customer = "John Doe",
                branch = "Store 1",
                items = new[]
                {
                    new { productId = "0195a090-a2f3-71bc-bfb2-44b4d6e759a3", quantity = 5, unitPrice = 100 }
                }
            };
            var response = await _client.PostAsJsonAsync("/api/sales", requestBody);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "SalesController: GET /api/sales returns 200")]
        public async Task GetSales_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/sales");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
