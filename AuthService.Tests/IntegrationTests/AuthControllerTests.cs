using System.Text;
using System.Text.Json;
using AuthService.Api.Data;
using AuthService.Api.Dto.Request;
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

        // var findUserByIdDto = new FindUserByIdRequestDto
        // {
        //     UserId = await registerResponse.Content.ReadAsStringAsync()
        // };
        //
        // var deleteContent = new StringContent(
        //     JsonSerializer.Serialize(findUserByIdDto), 
        //     Encoding.UTF8, "application/json"
        // );
        //
        // var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/admin/user/delete");
        // deleteRequest.Content = deleteContent;
        //
        // var deleteResponse = await client.SendAsync(deleteRequest);
        // Assert.Equal(CodesData.SuccessCode, (int)deleteResponse.StatusCode);
    }
}