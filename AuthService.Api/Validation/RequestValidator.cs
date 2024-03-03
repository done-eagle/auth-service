using AuthService.Api.Dto.Request;

namespace AuthService.Api.Validation;

internal static class RequestValidator
{
    public static void ValidateRequest(CreateUserRequestDto userRequestDto)
    {
        if (userRequestDto.Username is null or "")
            throw new ApplicationException("Incorrect username");
        if (userRequestDto.FirstName is null or "")
            throw new ApplicationException("Incorrect firstname");
        if (userRequestDto.LastName is null or "")
            throw new ApplicationException("Incorrect lastname");
        if (userRequestDto.PhoneNumber is null or "")
            throw new ApplicationException("Incorrect phone number");
        if (userRequestDto.Email is null or "")
            throw new ApplicationException("Incorrect email");
        if (userRequestDto.Password is null or "")
            throw new ApplicationException("Incorrect password");
    }

    public static void ValidateRequest(GetAccessTokenRequestDto userRequestDto)
    {
        if (userRequestDto.AuthCode is null or "")
            throw new ApplicationException("Incorrect authorization_code");
        if (userRequestDto.CodeVerifier is null or "")
            throw new ApplicationException("Incorrect code_verifier");
    }

    public static void ValidateRequest(FindUserByIdRequestDto userDto)
    {
        if (userDto.UserId is null or "")
            throw new ApplicationException("Incorrect userId");
    }
    
    public static void ValidateRequest(UpdateUserRequestDto userRequestDto)
    {
        if (userRequestDto.UserId is null or "")
            throw new ApplicationException("Incorrect userId");
        if (userRequestDto.Email is null or "")
            throw new ApplicationException("Incorrect email");
        if (userRequestDto.Password is null or "")
            throw new ApplicationException("Incorrect password");
    }
}