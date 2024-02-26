using AuthService.Api.Сonverters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DefaultController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Ok("Login");
    }
    
    [HttpGet("add")]
    [Authorize(Roles = "admin")]
    public IActionResult Add()
    {
        return Ok("add-work");
    }
    
    [HttpGet("view")]
    [Authorize(Roles = "user")]
    public IActionResult View()
    {
        return Ok("view-work");
    }
    
    [HttpGet("delete")]
    [Authorize(Roles = "admin")]
    public IActionResult Delete()
    {
        return Ok("delete-work");
    }
}