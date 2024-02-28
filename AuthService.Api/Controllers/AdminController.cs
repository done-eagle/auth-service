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

    public AdminController(IKeycloakUtils keycloakUtils)
    {
        _keycloakUtils = keycloakUtils; 
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserDto userDto)
    {
        try
        {
            ValidateRequest(userDto);
            var createdUserId = await _keycloakUtils.CreateUser(userDto);
            
            Console.WriteLine("User created with userId: " + createdUserId);
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