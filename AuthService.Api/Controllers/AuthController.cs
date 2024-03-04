using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validation;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IKeycloakUtils _keycloakUtils;
    private const int SuccessCode = 200;

    public AuthController(IKeycloakUtils keycloakUtils)
    {
        _keycloakUtils = keycloakUtils;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            var createdUserId = await _keycloakUtils.CreateUser(userRequestDto);
            Console.WriteLine($"User created with userId: {createdUserId}");
    
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException ex)
        {
            var statusCode = (int)ex.StatusCode!;
            return StatusCode(statusCode);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromBody] GetAccessTokenRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            var accessTokenResponse = await _keycloakUtils.GetAccessToken(userRequestDto);

            if (accessTokenResponse.StatusCode != SuccessCode)
                return StatusCode(accessTokenResponse.StatusCode);
            
            return Content(accessTokenResponse.AccessToken, "application/json");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException ex)
        {
            var statusCode = (int)ex.StatusCode!;
            return StatusCode(statusCode);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccessTokenByRefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(refreshTokenRequestDto);
            var accessTokenResponse = await _keycloakUtils.GetAccessTokenByRefreshToken(refreshTokenRequestDto);
            
            if (accessTokenResponse.StatusCode != SuccessCode)
                return StatusCode(accessTokenResponse.StatusCode);
            
            return Content(accessTokenResponse.AccessToken, "application/json");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException ex)
        {
            var statusCode = (int)ex.StatusCode!;
            return StatusCode(statusCode);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(refreshTokenRequestDto);
            var logoutResponse = await _keycloakUtils.LogoutUser(refreshTokenRequestDto);
            
            return StatusCode(logoutResponse.StatusCode);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException ex)
        {
            var statusCode = (int)ex.StatusCode!;
            return StatusCode(statusCode);
        }
    }
}