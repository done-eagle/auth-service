using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;

namespace AuthService.Api.Keycloak;

public interface IKeycloakUtils
{
    Task<string> CreateUser(CreateUserRequestDto createUserRequestDto);
    Task<GetAccessTokenResponseDto> GetAccessToken(GetAccessTokenRequestDto getAccessTokenRequestDto);
    Task<GetAccessTokenResponseDto> GetAccessTokenByRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    Task<GetAccessTokenResponseDto> LogoutUser(RefreshTokenRequestDto refreshTokenRequestDto);
    Task<FindUserByIdResponseDto> FindById(FindUserByIdRequestDto findUserByIdRequestDto);
    Task UpdateUser(UpdateUserRequestDto updateUserRequestDto);
    Task DeleteUser(FindUserByIdRequestDto findUserByIdRequestDto);
}