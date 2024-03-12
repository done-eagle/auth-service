using Newtonsoft.Json;

namespace AuthService.Api.Dto.Response;

public class AccessTokenResponseDto
{
    [JsonProperty("access_token")]
    public string AccessToken { get; init; } = null!;
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; init; }
    [JsonProperty("refresh_expires_in")]
    public long RefreshExpiresIn { get; init; }
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; init; } = null!;
    [JsonProperty("token_type")]
    public string TokenType { get; init; } = null!;
    [JsonProperty("id_token")]
    public string IdToken { get; init; } = null!;
    [JsonProperty("not-before-policy")]
    public int NotBeforePolicy { get; init; }
    [JsonProperty("session_state")]
    public string SessionState { get; init; } = null!;
    [JsonProperty("scope")]
    public string Scope { get; init; } = null!;
}