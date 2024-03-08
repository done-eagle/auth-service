using AuthService.Api.Controllers;
using AuthService.Api.Dto.Request;
using AuthService.Api.Keycloak;
using Moq;

namespace AuthService.Tests;

public class AuthControllerTests
{
    [Fact]
    public void Register_ReturnsOkResult()
    {
        // Arrange
        // var keycloakUtilsMock = new Mock<IKeycloakUtils>();
        // keycloakUtilsMock.Setup(x => x.CreateUser(new CreateUserRequestDto
        //     {
        //         Username = "berserk",
        //         FirstName = "Pavel",
        //         LastName = "Ivanov",
        //         PhoneNumber = "89058679814",
        //         Email = "pavlov@gmail.com",
        //         Password = "df224kl"
        //     }))
        //     .Returns(Task.FromResult("userId"));
        //
        // var authController = new AuthController(keycloakUtilsMock.Object);
    }
}