namespace AuthService.Api.Dto.Response;

public class GetAccessTokenResponseDto
{
    public int StatusCode { get; init; }
    public string AccessToken { get; init; }
    
    public GetAccessTokenResponseDto(int statusCode, string accessToken)
    {
        StatusCode = statusCode;
        AccessToken = accessToken;
    }
}