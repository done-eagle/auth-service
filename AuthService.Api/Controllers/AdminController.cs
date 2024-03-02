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
    private readonly IConfiguration _config;
    private const string RealmConfigKey = "Keycloak:Realm";

    public AdminController(IKeycloakUtils keycloakUtils, IConfiguration config)
    {
        _keycloakUtils = keycloakUtils;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(createUserRequestDto);
            var createdUserId = await _keycloakUtils.CreateUser(_config[RealmConfigKey], createUserRequestDto);
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
    public async Task<IActionResult> FindById([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(findUserByIdRequestDto);
            var userResponseDto = await _keycloakUtils.FindById(_config[RealmConfigKey], findUserByIdRequestDto);
            return Ok(userResponseDto);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {findUserByIdRequestDto.UserId} not found");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(updateUserRequestDto);
            await _keycloakUtils.UpdateUser(_config[RealmConfigKey], updateUserRequestDto);
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {updateUserRequestDto.UserId} not found");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(findUserByIdRequestDto);
            await _keycloakUtils.DeleteUser(_config[RealmConfigKey], findUserByIdRequestDto);
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {findUserByIdRequestDto.UserId} not found");
        }
    }
}