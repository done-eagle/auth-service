namespace AuthService.Api.Dto.Response;

public class FindUserByIdResponseDto
{
    public int StatusCode { get; init; }
    public UserResponseDto UserResponseDto { get; init; }
    public FindUserByIdResponseDto(int statusCode, UserResponseDto userResponseDto)
    {
        StatusCode = statusCode;
        UserResponseDto = userResponseDto;
    }
}