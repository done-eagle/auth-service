namespace AuthService.Api.Dto.Response;

public class FindUserByIdResponseDto
{
    public string Id { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
}