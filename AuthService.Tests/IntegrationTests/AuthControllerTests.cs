using System.Text;
using AuthService.Api.Data;
using AuthService.Tests.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace AuthService.Tests.IntegrationTests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private string UserId { get; set; } = null!;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClientFactory = factory.Services.GetRequiredService<IHttpClientFactory>();
        _config = factory.Services.GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async Task RegisterUser_ReturnsCreatedResult()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var registerContent = new StringContent(
            JsonConvert.SerializeObject(AuthControllerTestData.CreateUserDto), 
            Encoding.UTF8, "application/json"
        );
        
        // Act
        var registerResponse = await client.PostAsync("/api/auth/register", registerContent);
        UserId = await registerResponse.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(CodesData.CreatedCode, (int)registerResponse.StatusCode);
    }
    
    public void Dispose()
    {
        TestHelper.CleanupAsync(_factory, _httpClientFactory, _config, UserId).GetAwaiter().GetResult();
    }
}