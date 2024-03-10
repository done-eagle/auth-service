using AuthService.Api.Data;
using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using AuthService.Api.Validators;
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
        var validator = new CreateUserDtoValidator();
        var validationResult = await validator.ValidateAsync(createUserRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var responseDto = await _keycloakUtils.CreateUser(createUserRequestDto);
        return StatusCode(responseDto.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> FindById([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        var validator = new FindUserByIdDtoValidator();
        var validationResult = await validator.ValidateAsync(findUserByIdRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var responseDto = await _keycloakUtils.FindById(findUserByIdRequestDto);
        
        if (responseDto.StatusCode != CodesData.SuccessCode)
            return StatusCode(responseDto.StatusCode);
        
        return Ok(responseDto.UserResponseDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        var validator = new UpdateUserDtoValidator();
        var validationResult = await validator.ValidateAsync(updateUserRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var responseDto = await _keycloakUtils.UpdateUser(updateUserRequestDto);
        return StatusCode(responseDto.StatusCode);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] FindUserByIdRequestDto findUserByIdRequestDto)
    {
        var validator = new FindUserByIdDtoValidator();
        var validationResult = await validator.ValidateAsync(findUserByIdRequestDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        await _keycloakUtils.DeleteUser(findUserByIdRequestDto);
        return Ok();
    }
}