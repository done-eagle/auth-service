using AuthService.Api.Dto.Request;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(string realm, CreateUserRequestDto createUserRequestDto);
    Task<User> FindById(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
    Task UpdateUser(string realm, UpdateUserRequestDto updateUserRequestDto);
    Task DeleteUser(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
}