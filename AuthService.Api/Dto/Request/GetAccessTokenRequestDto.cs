namespace AuthService.Api.Dto.Request;

public class GetAccessTokenRequestDto
{
    public string AuthCode { get; init; } = null!;
    public string CodeVerifier { get; init; } = null!;
}