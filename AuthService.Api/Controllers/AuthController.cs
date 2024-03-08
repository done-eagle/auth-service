using AuthService.Api.Data;
using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var responseDto = await _keycloakUtils.CreateUser(userRequestDto);
        return StatusCode(responseDto.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromBody] GetAccessTokenRequestDto userRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var accessTokenResponse = await _keycloakUtils.GetAccessToken(userRequestDto);

        if (accessTokenResponse.StatusCode != CodesData.SuccessCode)
            return StatusCode(accessTokenResponse.StatusCode);
        
        return Content(accessTokenResponse.AccessToken, "application/json");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccessTokenByRefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var accessTokenResponse = await _keycloakUtils.GetAccessTokenByRefreshToken(refreshTokenRequestDto);
        
        if (accessTokenResponse.StatusCode != CodesData.SuccessCode)
            return StatusCode(accessTokenResponse.StatusCode);
        
        return Content(accessTokenResponse.AccessToken, "application/json");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var logoutResponse = await _keycloakUtils.LogoutUser(refreshTokenRequestDto);
        
        return StatusCode(logoutResponse.StatusCode);
    }
}