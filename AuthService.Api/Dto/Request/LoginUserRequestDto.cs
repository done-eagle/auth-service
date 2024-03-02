namespace AuthService.Api.Dto.Request;

public class LoginUserRequestDto
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string CodeChallenge { get; init; } = null!;
}