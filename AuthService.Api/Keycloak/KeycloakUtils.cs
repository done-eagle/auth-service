using AuthService.Api.Dto;
using Keycloak.Net;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public class KeycloakUtils : IKeycloakUtils
{
    private readonly KeycloakClient _keycloakClient;
    
    public KeycloakUtils(string serverUrl, string realm, string clientId, string clientSecret)
    {
        _keycloakClient = new KeycloakClient(
            serverUrl, 
            clientSecret, 
            new KeycloakOptions(
                prefix: "/auth",
                adminClientId: clientId, 
                authenticationRealm: realm
            ));
    }

    public async Task<bool> CreateUser(CreateUserDto createUserDto)
    {
        var credential = CreatePasswordCredentials(createUserDto.Password);
        var user = new User
        {
            UserName = createUserDto.Username,
            Credentials = new[] { credential },
            Email = createUserDto.Email,
            Enabled = true,
            EmailVerified = false
        };
        var response = await _keycloakClient.CreateUserAsync("memeapp-realm", user);

        return response;
    }

    private Credentials CreatePasswordCredentials(string password)
    {
        return new Credentials
        {
            Temporary = false,
            Type = "password",
            Value = password
        };
    }
}