namespace AuthService.Api.Dto.Request;

public class UpdateUserRequestDto
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}