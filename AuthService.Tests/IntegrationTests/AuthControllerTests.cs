using System.Text;
using AuthService.Tests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AuthService.Tests.IntegrationTests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private string _userId = null!;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClientFactory = factory.Services.GetRequiredService<IHttpClientFactory>();
        _config = factory.Services.GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async Task ComplexSession()
    {
        /* Регистрация */
        
        // Arrange
        using var client = _factory.CreateClient();
        var registerContent = new StringContent(
            JsonConvert.SerializeObject(AuthControllerTestData.CreateUserDto), 
            Encoding.UTF8, "application/json"
        );
        
        // Act
        var registerResponse = await client.PostAsync("/api/auth/register", registerContent);
        _userId = await registerResponse.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(StatusCodes.Status201Created, (int)registerResponse.StatusCode);
        
        /* Логин */
        
        // Arrange
        var codeVerifier = PkceGen.GenerateCodeVerifier();
        var codeChallenge = PkceGen.GenerateCodeChallenge(codeVerifier);
        
        var authenticationRequest = new StringBuilder(_config["Keycloak:AuthorizationUrl"]);
        authenticationRequest.Append($"?client_id={_config["Keycloak:ClientId"]}");
        authenticationRequest.Append("&response_type=code");
        authenticationRequest.Append($"&redirect_uri={_config["Keycloak:RedirectUri"]}");
        authenticationRequest.Append("&scope=openid");
        authenticationRequest.Append("&code_challenge_method=S256");
        authenticationRequest.Append($"&code_challenge={codeChallenge}");
        
        var authenticationUrl = authenticationRequest.ToString();
        
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(authenticationUrl);
        
        var loginField = driver.FindElement(By.Id("username"));
        loginField.SendKeys(AuthControllerTestData.CreateUserDto.Username);
        
        var passwordField = driver.FindElement(By.Id("password"));
        passwordField.SendKeys(AuthControllerTestData.CreateUserDto.Password);
        
        var submitButton = driver.FindElement(By.Id("kc-login"));
        submitButton.Click();
        
        var redirectUrl = driver.Url;
        driver.Quit();
        
        var uri = new Uri(redirectUrl);
        var queryString = uri.Query;
        var parameters = queryString.TrimStart('?').Split('&');
        var authCode = "";
        
        foreach (var parameter in parameters)
        {
            var parts = parameter.Split('=');
            if (parts.Length == 2 && parts[0] == "code")
                authCode = parts[1];
        }
        
        var loginUrl = $"/api/auth/login?authcode={authCode}&codeverifier={codeVerifier}";
        
        // Act
        var loginResponse = await client.GetAsync(loginUrl);
        var responseContent = await loginResponse.Content.ReadAsStringAsync();
        var accessTokenDto = JsonConvert.DeserializeObject<AccessTokenResponseTestDto>(responseContent);
        
        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)loginResponse.StatusCode);
        
        /* Обновление access_token по refresh_token */
        
        // Arrange
        var refreshTokenUrl = $"/api/auth/getAccessTokenByRefreshToken?refreshtoken={accessTokenDto!.RefreshToken}";
        
        // Act
        var accessTokenUpdateResponse = await client.GetAsync(refreshTokenUrl);
        
        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)accessTokenUpdateResponse.StatusCode);
        
        /* Логаут */
        
        // Arrange
        var logoutUrl = $"/api/auth/logout?refreshtoken={accessTokenDto!.RefreshToken}";
        
        // Act
        var logoutResponse = await client.GetAsync(logoutUrl);
        
        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)logoutResponse.StatusCode);
    }
    
    public void Dispose()
    {
        TestHelper.CleanupAsync(_factory, _httpClientFactory, _config, _userId).GetAwaiter().GetResult();
    }
}