using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
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
        return Ok("Login");
    }
}