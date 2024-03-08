namespace AuthService.Api.Dto.Response;

public class KeycloakResponseDto
{
    public int StatusCode { get; init; }
    
    public KeycloakResponseDto(int statusCode)
    {
        StatusCode = statusCode;
    }
}