using AuthService.Api.Dto.Request;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(string realm, CreateUserRequestDto createUserRequestDto);
    Task<User> FindById(string realm, string userId);
    Task UpdateUser(string realm, UpdateUserRequestDto createUserRequestDto);
    Task DeleteUser(string realm, string userId);

    // Task AddRoles(string realm, string userId, List<string> roles);
}