using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace NegotiationApp.Tests
{
    public class AuthTestController : IClassFixture<CustomWebAppFactor<Program>>
    {
        private readonly HttpClient _client;

        public AuthTestController(CustomWebAppFactor<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnOk()
        {
            var registerDto = new { Username = "testuser", Password = "Test@123" };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/register", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_ShouldReturnToken()
        {
            var registerDto = new { Username = "mnosel", Password = "zaq1@WSX" };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Auth/register", content);

            var loginDto = new { Username = "mnosel", Password = "zaq1@WSX" };
            content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("token");
        }
    }
}
