using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<KeycloakResponseDto> CreateUser(CreateUserRequestDto createUserRequestDto);
    Task<GetAccessTokenResponseDto> GetAccessToken(GetAccessTokenRequestDto getAccessTokenRequestDto);
    Task<GetAccessTokenResponseDto> GetAccessTokenByRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    Task<KeycloakResponseDto> LogoutUser(RefreshTokenRequestDto refreshTokenRequestDto);
    Task<FindUserByIdResponseDto> FindById(FindUserByIdRequestDto findUserByIdRequestDto);
    Task<KeycloakResponseDto> UpdateUser(UpdateUserRequestDto updateUserRequestDto);
    Task<KeycloakResponseDto> DeleteUser(FindUserByIdRequestDto findUserByIdRequestDto);
}