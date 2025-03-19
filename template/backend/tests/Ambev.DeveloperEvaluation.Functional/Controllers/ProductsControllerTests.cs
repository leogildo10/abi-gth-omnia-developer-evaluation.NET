using Xunit;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "ProductsController: POST /api/products returns 200/201")]
        public async Task PostProduct_ShouldReturn200Or201()
        {
            var requestBody = new
            {
                title = "TestProd",
                price = 10.5,
                description = "Desc",
                category = "Cat"
            };
            var response = await _client.PostAsJsonAsync("/api/products", requestBody);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "ProductsController: GET /api/products returns 200")]
        public async Task GetProducts_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
