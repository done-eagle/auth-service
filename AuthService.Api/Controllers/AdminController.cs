using AuthService.Api.Dto;
using AuthService.Api.Keycloak;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Authorize(Roles = "admin")]
[Route("api/[controller]/user/[action]")]
public class AdminController : ControllerBase
{
    private readonly IKeycloakUtils _keycloakUtils;
    private readonly IConfiguration _config;
    private const string RealmConfigKey = "Keycloak:Realm";
    private const string UserRole = "user";

    public AdminController(IKeycloakUtils keycloakUtils, IConfiguration config)
    {
        _keycloakUtils = keycloakUtils;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserDto userDto)
    {
        try
        {
            ValidateRequest(userDto);
            var createdUserId = await _keycloakUtils.CreateUser(_config[RealmConfigKey], userDto);
            Console.WriteLine("User created with userId: " + createdUserId);

            var defaultRoles = new List<string> { UserRole };
            await _keycloakUtils.AddRoles(_config[RealmConfigKey], createdUserId, defaultRoles);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    private void ValidateRequest(CreateUserDto createUserDto)
    {
        if (createUserDto.Username.Equals(null) || createUserDto.Username.Equals(""))
            throw new ArgumentException("Incorrect username");
        if (createUserDto.Email.Equals(null) || createUserDto.Email.Equals(""))
            throw new ArgumentException("Incorrect email");
        if (createUserDto.Password.Equals(null) || createUserDto.Password.Equals(""))
            throw new ArgumentException("Incorrect password");
    }
}