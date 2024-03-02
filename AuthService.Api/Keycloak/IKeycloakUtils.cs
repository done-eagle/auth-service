using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(string realm, CreateUserRequestDto createUserRequestDto);
    Task<FindUserByIdResponseDto> FindById(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
    Task UpdateUser(string realm, UpdateUserRequestDto updateUserRequestDto);
    Task DeleteUser(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
}