using System.Text;
using AuthService.Api.Data;
using AuthService.Tests.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace AuthService.Tests.IntegrationTests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private string UserId { get; set; } = null!;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
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
        TestHelper.CleanupAsync(_factory, UserId).GetAwaiter().GetResult();
    }
}