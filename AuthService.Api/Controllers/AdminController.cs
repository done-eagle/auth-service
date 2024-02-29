using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using AuthService.Api.Keycloak;
using AuthService.Api.Validation;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private const string RealmConfigKey = "Keycloak:Realm";

    public AdminController(IKeycloakUtils keycloakUtils, IConfiguration config, IMapper mapper)
    {
        _keycloakUtils = keycloakUtils;
        _config = config;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserRequestDto userRequestDto)
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
    public async Task<IActionResult> FindById([FromBody] FindUserByIdRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            var user = await _keycloakUtils.FindById(_config[RealmConfigKey], userRequestDto.UserId);
            var userResponseDto = _mapper.Map<FindUserByIdResponseDto>(user);
            return Ok(userResponseDto);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {userRequestDto.UserId} not found");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            await _keycloakUtils.UpdateUser(_config[RealmConfigKey], userRequestDto);
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {userRequestDto.UserId} not found");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserRequestDto userRequestDto)
    {
        try
        {
            RequestValidator.ValidateRequest(userRequestDto);
            await _keycloakUtils.DeleteUser(_config[RealmConfigKey], userRequestDto.UserId);
            return Ok();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FlurlHttpException)
        {
            return NotFound($"User with userId: {userRequestDto.UserId} not found");
        }
    }
}