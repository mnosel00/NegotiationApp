using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Tests
{
    public class ProductTestController : IClassFixture<CustomWebAppFactor<Program>>
    {
        private readonly HttpClient _client;

        public ProductTestController(CustomWebAppFactor<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/Product");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnCreated()
        {
            var token = await GetAdminToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var productDto = new { Name = "mleko", Price = 99.1 };
            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Product", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private async Task<string> GetAdminToken()
        {
            var registerDto = new { Username = "admin", Password = "Admin@123" };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Auth/register", content);

            var loginDto = new { Username = "admin", Password = "Admin@123" };
            content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            if (tokenResponse != null && tokenResponse.TryGetValue("token", out var token))
            {
                return token;
            }

            throw new Exception("Token not found in the response");
        }
    }
}
