using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Authorize(Roles = "user")]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult GetResource()
    {
        return Ok("Resource");
    }
}