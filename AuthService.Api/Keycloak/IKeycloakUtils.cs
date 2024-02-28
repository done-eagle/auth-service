using AuthService.Api.Dto;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(CreateUserDto createUserDto);
}