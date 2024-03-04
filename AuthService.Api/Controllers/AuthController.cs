using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validation;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
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
            var authCode = await _keycloakUtils.GetAccessToken(userRequestDto);
            return Content(authCode, "application/json");
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
            var token = await _keycloakUtils.GetAccessTokenByRefreshToken(refreshTokenRequestDto);
            return Content(token, "application/json");
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
            await _keycloakUtils.LogoutUser(refreshTokenRequestDto);
            return Ok("Logout successful");
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