namespace AuthService.Api.Dto.Response;

public class AccessTokenResponseDto
{
    public long ExpiresIn { get; init; }
    public string IdToken { get; init; } = null!;
    public int NotBeforePolicy { get; init; }
    public string RefreshToken { get; init; } = null!;
    public string Scope { get; init; } = null!;
    public string SessionState { get; init; } = null!;
    public string AccessToken { get; init; } = null!;
    public string TokenType { get; init; } = null!;
}