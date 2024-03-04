using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validation;
using Flurl.Http;
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
    public async Task<IActionResult> Add([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(createUserRequestDto);
            var createdUserId = await _keycloakUtils.CreateUser(createUserRequestDto);
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
    public async Task<IActionResult> FindById([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(findUserByIdRequestDto);
            var userResponseDto = await _keycloakUtils.FindById(findUserByIdRequestDto);
            return Ok(userResponseDto);
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(updateUserRequestDto);
            await _keycloakUtils.UpdateUser(updateUserRequestDto);
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

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(findUserByIdRequestDto);
            await _keycloakUtils.DeleteUser(findUserByIdRequestDto);
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
}