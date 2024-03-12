using System.Net.Http.Headers;
using System.Text;
using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AuthService.Tests;

internal static class TestHelper
{
    internal static async Task CleanupAsync(WebApplicationFactory<Program> factory, 
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        string userId)
    {
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", config["Keycloak:ManageClientId"] },
            { "client_secret", config["Keycloak:ClientSecret"] }
        });

        using var httpClient = httpClientFactory.CreateClient();
        var tokenResponse = await httpClient.PostAsync(config["Keycloak:TokenUrl"], requestContent);
        
        var responseContent = await tokenResponse.Content.ReadAsStringAsync();
        var accessTokenDto = JsonConvert.DeserializeObject<AccessTokenResponseDto>(responseContent);

        var findUserByIdDto = new FindUserByIdRequestDto { UserId = userId };
        
        var deleteContent = new StringContent(
            JsonConvert.SerializeObject(findUserByIdDto), 
            Encoding.UTF8, "application/json"
        );
        
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/admin/user/delete");
        deleteRequest.Content = deleteContent;
        
        deleteRequest.Headers.Authorization = 
            new AuthenticationHeaderValue("Bearer", accessTokenDto!.AccessToken);
        
        using var client = factory.CreateClient();
        await client.SendAsync(deleteRequest);
    }
}