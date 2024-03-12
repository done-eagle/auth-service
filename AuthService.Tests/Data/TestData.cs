using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;

namespace AuthService.Tests.Data;

internal static class TestData
{
    internal static readonly CreateUserRequestDto CreateUserDto = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = "Ivan",
        LastName = "Ivanov",
        PhoneNumber = "89058679814",
        Email = "ivanov@gmail.com",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongUsername = new CreateUserRequestDto
    {
        Username = "",
        FirstName = "Ivan",
        LastName = "Ivanov",
        PhoneNumber = "89058679814",
        Email = "ivanov@gmail.com",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongFirstName = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = null!,
        LastName = "Ivanov",
        PhoneNumber = "89058679814",
        Email = "ivanov@gmail.com",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongLastName = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = "Ivan",
        LastName = "",
        PhoneNumber = "89058679814",
        Email = "ivanov@gmail.com",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongPhoneNumber = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = "Ivan",
        LastName = "Ivanov",
        PhoneNumber = null!,
        Email = "ivanov@gmail.com",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongEmail = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = "Ivan",
        LastName = "Ivanov",
        PhoneNumber = "89058679814",
        Email = "ivanov01",
        Password = "df224kl2aw"
    };
    
    internal static readonly CreateUserRequestDto CreateUserDtoWrongPassword = new CreateUserRequestDto
    {
        Username = "ivanov01",
        FirstName = "Ivan",
        LastName = "Ivanov",
        PhoneNumber = "89058679814",
        Email = "ivanov@gmail.com",
        Password = "df224"
    };
    
    internal static readonly GetAccessTokenRequestDto GetAccessTokenDto = new GetAccessTokenRequestDto
    {
        AuthCode = "authorization_code",
        CodeVerifier = "code_verifier"
    };
    
    internal static readonly GetAccessTokenRequestDto GetAccessTokenDtoWrongAuthCode = new GetAccessTokenRequestDto
    {
        AuthCode = "",
        CodeVerifier = "code_verifier"
    };
    
    internal static readonly GetAccessTokenRequestDto GetAccessTokenDtoWrongCodeVerifier = new GetAccessTokenRequestDto
    {
        AuthCode = "authorization_code",
        CodeVerifier = null!
    };
    
    internal static readonly RefreshTokenRequestDto RefreshTokenDto = new RefreshTokenRequestDto
    {
        RefreshToken = "refresh_token"
    };
    
    internal static readonly RefreshTokenRequestDto RefreshTokenDtoWrongRT = new RefreshTokenRequestDto
    {
        RefreshToken = ""
    };
    
    internal static readonly AccessTokenResponseDto AccessTokenResponseDto = new AccessTokenResponseDto
    {
        ExpiresIn = 10L,
        IdToken = "IdToken",
        NotBeforePolicy = 100,
        RefreshExpiresIn = 10L,
        RefreshToken = "RefreshToken",
        Scope = "Scope",
        SessionState = "SessionState",
        AccessToken = "AccessToken",
        TokenType = "TokenType"
    };
}