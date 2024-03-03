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
    [AllowAnonymous]
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
        catch (Exception ex)
        {
            //return Conflict("User with such data already exist");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [AllowAnonymous]
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
    }
    
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Logout()
    {
        return Ok("Logout");
    }
}