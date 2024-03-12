using AuthService.Api.Data;
using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IKeycloakUtils _keycloakUtils;

    public AuthController(IKeycloakUtils keycloakUtils)
    {
        _keycloakUtils = keycloakUtils;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto userRequestDto)
    {
        var validator = new CreateUserDtoValidator();
        var validationResult = await validator.ValidateAsync(userRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var responseDto = await _keycloakUtils.CreateUser(userRequestDto);
        return StatusCode(responseDto.StatusCode, responseDto.Id);
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromBody] GetAccessTokenRequestDto userRequestDto)
    {
        var validator = new GetAccessTokenDtoValidator();
        var validationResult = await validator.ValidateAsync(userRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var accessTokenResponse = await _keycloakUtils.GetAccessToken(userRequestDto);
        return StatusCode(accessTokenResponse.StatusCode, accessTokenResponse.AccessTokenResponseDto);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccessTokenByRefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var validator = new RefreshTokenDtoValidator();
        var validationResult = await validator.ValidateAsync(refreshTokenRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var accessTokenResponse = await _keycloakUtils.GetAccessTokenByRefreshToken(refreshTokenRequestDto);
        return StatusCode(accessTokenResponse.StatusCode, accessTokenResponse.AccessTokenResponseDto);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var validator = new RefreshTokenDtoValidator();
        var validationResult = await validator.ValidateAsync(refreshTokenRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var logoutResponse = await _keycloakUtils.LogoutUser(refreshTokenRequestDto);
        
        return StatusCode(logoutResponse.StatusCode);
    }
}