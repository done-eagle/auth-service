namespace AuthService.Tests.Data;

public class AccessTokenResponseTestDto
{
    public string AccessToken { get; init; } = null!;
    public long ExpiresIn { get; init; }
    public long RefreshExpiresIn { get; init; }
    public string RefreshToken { get; init; } = null!;
    public string TokenType { get; init; } = null!;
    public string IdToken { get; init; } = null!;
    public int NotBeforePolicy { get; init; }
    public string SessionState { get; init; } = null!;
    public string Scope { get; init; } = null!;
}