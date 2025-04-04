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
    public class NegotiationTestController : IClassFixture<CustomWebAppFactor<Program>>
    {
        private readonly HttpClient _client;

        public NegotiationTestController(CustomWebAppFactor<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllNegotiations_ShouldReturnOk()
        {
            var token = await GetAdminToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/Negotiation");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddNegotiation_ShouldReturnCreated()
        {
            var negotiationDto = new { ProductId = 1, ProposedPrice = 100.0M };
            var content = new StringContent(JsonConvert.SerializeObject(negotiationDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Negotiation", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private async Task<string> GetAdminToken()
        {
            var registerDto = new { Username = "mnosel00", Password = "zaq1@WSX" };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Auth/register", content);

            var loginDto = new { Username = "mnosel00", Password = "zaq1@WSX" };
            content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString)["token"];
            return token;
        }
    }
}
