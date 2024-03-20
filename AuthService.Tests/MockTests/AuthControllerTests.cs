using AuthService.Api.Controllers;
using AuthService.Api.Dto.Response;
using AuthService.Api.Keycloak;
using AuthService.Tests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuthService.Tests.MockTests;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_IncorrectUserName()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongUsername);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_IncorrectFirstName()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongFirstName);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_IncorrectLastName()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongLastName);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_IncorrectPhoneNumber()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongPhoneNumber);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_IncorrectEmail()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongEmail);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_IncorrectPassword()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDtoWrongPassword);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Login_IncorrectAuthorizationCode()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Login(AuthControllerTestData.GetAccessTokenDtoWrongAuthCode);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Login_IncorrectCodeVerifier()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Login(AuthControllerTestData.GetAccessTokenDtoWrongCodeVerifier);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task GetAccessTokenByRefreshToken_IncorrectRefreshToken()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.GetAccessTokenByRefreshToken(AuthControllerTestData.RefreshTokenDtoWrongRT);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Logout_IncorrectRefreshToken()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Logout(AuthControllerTestData.RefreshTokenDtoWrongRT);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Register_ReturnsCreatedResult()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.CreateUser(AuthControllerTestData.CreateUserDto))
            .Returns(Task.FromResult(new CreateUserResponseDto(StatusCodes.Status201Created, "userId")));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDto);
        
        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal("userId", objectResult.Value);
    }

    [Fact]
    public async Task Register_ReturnsConflictResult()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.CreateUser(AuthControllerTestData.CreateUserDto))
            .Returns(Task.FromResult(new CreateUserResponseDto(StatusCodes.Status409Conflict, "")));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Register(AuthControllerTestData.CreateUserDto);
        
        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, objectResult.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsOkResultWithToken()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.GetAccessToken(AuthControllerTestData.GetAccessTokenDto))
            .Returns(Task.FromResult(new GetAccessTokenResponseDto(StatusCodes.Status200OK, AuthControllerTestData.AccessTokenResponseDto)));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Login(AuthControllerTestData.GetAccessTokenDto);
        
        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(AuthControllerTestData.AccessTokenResponseDto, objectResult.Value);
    }
    
    [Fact]
    public async Task Login_ReturnsUnauthorizedResult()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.GetAccessToken(AuthControllerTestData.GetAccessTokenDto))
            .Returns(Task.FromResult(new GetAccessTokenResponseDto(StatusCodes.Status401Unauthorized, null!)));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Login(AuthControllerTestData.GetAccessTokenDto);
        
        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetAccessTokenByRefreshToken_ReturnsOkResultWithToken()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.GetAccessTokenByRefreshToken(AuthControllerTestData.RefreshTokenDto))
            .Returns(Task.FromResult(new GetAccessTokenResponseDto(StatusCodes.Status200OK, AuthControllerTestData.AccessTokenResponseDto)));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.GetAccessTokenByRefreshToken(AuthControllerTestData.RefreshTokenDto);
        
        // Assert
        var contentResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(AuthControllerTestData.AccessTokenResponseDto, contentResult.Value);
    }

    [Fact]
    public async Task GetAccessTokenByRefreshToken_ReturnsUnauthorizedResult()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.GetAccessTokenByRefreshToken(AuthControllerTestData.RefreshTokenDto))
            .Returns(Task.FromResult(new GetAccessTokenResponseDto(StatusCodes.Status401Unauthorized, null!)));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.GetAccessTokenByRefreshToken(AuthControllerTestData.RefreshTokenDto);
        
        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
    }

    [Fact]
    public async Task Logout_ReturnsNoContentResult()
    {
        // Arrange
        var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        keycloakUtilsMock.Setup(x => 
                x.LogoutUser(AuthControllerTestData.RefreshTokenDto))
            .Returns(Task.FromResult(new KeycloakResponseDto(StatusCodes.Status204NoContent)));
        
        var authController = new AuthController(keycloakUtilsMock.Object);
        
        // Act
        var result = await authController.Logout(AuthControllerTestData.RefreshTokenDto);
        
        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
    }
}