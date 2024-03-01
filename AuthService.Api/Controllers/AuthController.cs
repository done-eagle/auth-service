using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validation;
using AutoMapper;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IKeycloakUtils _keycloakUtils;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private const string RealmConfigKey = "Keycloak:Realm";

    public AuthController(IKeycloakUtils keycloakUtils, IConfiguration config, IMapper mapper)
    {
        _keycloakUtils = keycloakUtils;
        _config = config;
        _mapper = mapper;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            var createdUserId = await _keycloakUtils.CreateUser(_config[RealmConfigKey], userRequestDto);
            Console.WriteLine($"User created with userId: {createdUserId}");
    
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return Conflict("User with such data already exist");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return Ok("Login");
    }
    
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Logout()
    {
        return Ok("Logout");
    }
}