namespace AuthService.Api.Dto.Response;

public class UserResponseDto
{
    public string Id { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string Email { get; init; } = null!;
}