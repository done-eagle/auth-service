using AuthService.Api.Dto;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<bool> CreateUser(CreateUserDto createUserDto);
}