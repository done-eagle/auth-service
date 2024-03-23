namespace AuthService.Api.Dto.Request;

public class ChangeUserPasswordRequestDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
}