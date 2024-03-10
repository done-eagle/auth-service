using System.Text;
using System.Text.Json;
using AuthService.Api.Data;
using AuthService.Tests.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AuthService.Tests.IntegrationTests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegisterUser_ReturnsCreatedResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registerContent = new StringContent(
            JsonSerializer.Serialize(TestData.CreateUserDto), 
            Encoding.UTF8, "application/json"
        );
        
        // Act
        var registerResponse = await client.PostAsync("/api/auth/register", registerContent);
        
        // Assert
        Assert.Equal(CodesData.CreatedCode, (int)registerResponse.StatusCode);
    }
    
    [Fact]
    public async Task RegisterUser_ReturnsConflictResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registerContent = new StringContent(
            JsonSerializer.Serialize(TestData.CreateUserDto), 
            Encoding.UTF8, "application/json"
        );
        
        // Act
        await client.PostAsync("/api/auth/register", registerContent);
        var registerResponse = await client.PostAsync("/api/auth/register", registerContent);
        
        // Assert
        Assert.Equal(CodesData.ConflictCode, (int)registerResponse.StatusCode);
    }
}