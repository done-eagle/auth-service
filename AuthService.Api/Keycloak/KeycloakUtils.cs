using AuthService.Api.Dto.Request;
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

    public async Task<string> CreateUser(string realm, CreateUserRequestDto createUserRequestDto)
    {
        var credential = CreatePasswordCredentials(createUserRequestDto.Password);
        var user = new User
        {
            UserName = createUserRequestDto.Username,
            Credentials = new[] { credential },
            Email = createUserRequestDto.Email,
            Enabled = true,
            EmailVerified = false
        };
        var response = await _keycloakClient.CreateAndRetrieveUserIdAsync(realm, user);

        return response;
    }

    public async Task<User> FindById(string realm, string userId)
    {
        return await _keycloakClient.GetUserAsync(realm, userId);
    }

    public async Task UpdateUser(string realm, UpdateUserRequestDto updateUserRequestDto)
    {
        var user = await _keycloakClient.GetUserAsync(realm, updateUserRequestDto.UserId);
        var credential = CreatePasswordCredentials(updateUserRequestDto.Password);
        
        user.Email = updateUserRequestDto.Email;
        user.Credentials = new[] { credential };
        
        await _keycloakClient.UpdateUserAsync(realm, updateUserRequestDto.UserId, user);
    }

    public async Task DeleteUser(string realm, string userId)
    {
        await _keycloakClient.DeleteUserAsync(realm, userId);
    }

    // public async Task AddRoles(string realm, string userId, List<string> roles)
    // {
    //     var user = await _keycloakClient.GetUserAsync(realm, userId);
    //     
    //     if (user != null)
    //     {
    //         var existingRoles = user.RealmRoles?.ToList() ?? new List<string>();
    //         existingRoles.AddRange(roles);
    //         user.RealmRoles = existingRoles;
    //     
    //         // Сохраняем изменения в Keycloak
    //         await _keycloakClient.UpdateUserAsync(realm, userId, user);
    //     } 
    //     else
    //     {
    //         throw new ApplicationException($"User with id {userId} not found.");
    //     }
    // }

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