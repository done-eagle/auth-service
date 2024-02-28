using AuthService.Api.Dto;
using Keycloak.Net;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public class KeycloakUtils : IKeycloakUtils
{
    private readonly KeycloakClient _keycloakClient;
    
    public KeycloakUtils(string serverUrl, string clientId, string clientSecret)
    {
        _keycloakClient = new KeycloakClient(
            serverUrl, 
            clientSecret, 
            new KeycloakOptions(
                prefix: "/auth",
                adminClientId: clientId
            ));
    }

    public async Task<string> CreateUser(string realm, CreateUserDto createUserDto)
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
        var response = await _keycloakClient.CreateAndRetrieveUserIdAsync(realm, user);

        return response;
    }

    public async Task AddRoles(string realm, string userId, List<string> roles)
    {
        var user = await _keycloakClient.GetUserAsync(realm, userId);
        
        if (user != null)
        {
            var existingRoles = user.RealmRoles?.ToList() ?? new List<string>();
            existingRoles.AddRange(roles);
            user.RealmRoles = existingRoles;
        
            // Сохраняем изменения в Keycloak
            await _keycloakClient.UpdateUserAsync(realm, userId, user);
        } 
        else
        {
            throw new ApplicationException($"User with id {userId} not found.");
        }
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