namespace AuthService.Api.Dto.Response;

public class CreateUserResponseDto
{
    public int StatusCode { get; init; }
    public string Id { get; init; }
    
    public CreateUserResponseDto(int statusCode, string id)
    {
        StatusCode = statusCode;
        Id = id;
    }
}