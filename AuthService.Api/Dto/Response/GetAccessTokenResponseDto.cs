namespace AuthService.Api.Dto.Response;

public class GetAccessTokenResponseDto
{
    public int StatusCode { get; init; }
    public AccessTokenResponseDto AccessTokenResponseDto { get; init; }
    
    public GetAccessTokenResponseDto(int statusCode, AccessTokenResponseDto accessTokenResponseDto)
    {
        StatusCode = statusCode;
        AccessTokenResponseDto = accessTokenResponseDto;
    }
}