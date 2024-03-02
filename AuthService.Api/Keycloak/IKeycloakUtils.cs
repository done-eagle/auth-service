using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(string realm, CreateUserRequestDto createUserRequestDto);
    Task AuthenticateUser(string realm, LoginUserRequestDto loginUserRequestDto);
    Task<FindUserByIdResponseDto> FindById(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
    Task UpdateUser(string realm, UpdateUserRequestDto updateUserRequestDto);
    Task DeleteUser(string realm, FindUserByIdRequestDto findUserByIdRequestDto);
}