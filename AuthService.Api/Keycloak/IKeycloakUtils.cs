using AuthService.Api.Dto;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(string realm, CreateUserDto createUserDto);
    Task AddRoles(string realm, string userId, List<string> roles);
}