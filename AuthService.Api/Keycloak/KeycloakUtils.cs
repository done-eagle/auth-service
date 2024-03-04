using System.Net;
using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using AutoMapper;
using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Keycloak;

public class KeycloakUtils : IKeycloakUtils
{
    private readonly KeycloakClient _keycloakClient;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string RealmConfigKey = "Keycloak:Realm";
    
    public KeycloakUtils(string serverUrl, string clientId, string clientSecret, IMapper mapper, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _mapper = mapper;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _keycloakClient = new KeycloakClient(
            serverUrl, 
            clientSecret, 
            new KeycloakOptions(
                prefix: "/auth",
                adminClientId: clientId
            ));
    }

    public async Task<string> CreateUser(CreateUserRequestDto createUserRequestDto)
    {
        var credential = CreatePasswordCredentials(createUserRequestDto.Password);
        var user = _mapper.Map<User>(createUserRequestDto);
        
        user.Enabled = true;
        user.EmailVerified = false;
        user.Credentials = new[] { credential };
        
        var response = await _keycloakClient.CreateAndRetrieveUserIdAsync(_config[RealmConfigKey], user);

        return response;
    }

    public async Task<FindUserByIdResponseDto> FindById(FindUserByIdRequestDto findUserByIdRequestDto)
    {
        var user = await _keycloakClient.GetUserAsync(_config[RealmConfigKey], findUserByIdRequestDto.UserId);
        return _mapper.Map<FindUserByIdResponseDto>(user);
    }

    public async Task UpdateUser(UpdateUserRequestDto updateUserRequestDto)
    {
        var user = await _keycloakClient.GetUserAsync(_config[RealmConfigKey], updateUserRequestDto.UserId);
        var credential = CreatePasswordCredentials(updateUserRequestDto.Password);
        
        user.Email = updateUserRequestDto.Email;
        user.Credentials = new[] { credential };
        
        await _keycloakClient.UpdateUserAsync(_config[RealmConfigKey], updateUserRequestDto.UserId, user);
    }

    public async Task DeleteUser(FindUserByIdRequestDto findUserByIdRequestDto)
    {
        await _keycloakClient.DeleteUserAsync(_config[RealmConfigKey], findUserByIdRequestDto.UserId);
    }
    
    public async Task<string> GetAccessToken(GetAccessTokenRequestDto getAccessTokenRequestDto)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", _config["Keycloak:ClientId"] },
            { "code", getAccessTokenRequestDto.AuthCode },
            { "redirect_uri", _config["Keycloak:RedirectUri"] },
            { "code_verifier", getAccessTokenRequestDto.CodeVerifier },
        };

        var tokenResponse = await httpClient.PostAsync(_config["Keycloak:TokenUrl"], 
            new FormUrlEncodedContent(requestContent));

        if (!tokenResponse.IsSuccessStatusCode)
            throw new ApplicationException($"Access token not received. Status Code: {tokenResponse.StatusCode}");
            
        return await tokenResponse.Content.ReadAsStringAsync();
    }

    public async Task<string> GetAccessTokenByRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var checkRefreshToken = await CheckRefreshToken(refreshTokenRequestDto);
        
        if (!checkRefreshToken.IsSuccessStatusCode)
            throw new ApplicationException($"Invalid refresh token. Status Code: {checkRefreshToken.StatusCode}");
        
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", _config["Keycloak:ClientId"] },
            { "refresh_token", refreshTokenRequestDto.RefreshToken }
        });
        
        var tokenResponse = await httpClient.PostAsync(_config["Keycloak:TokenUrl"], requestContent);

        if (!tokenResponse.IsSuccessStatusCode)
            throw new ApplicationException($"Access token not received. Status Code: {tokenResponse.StatusCode}");
        
        return await tokenResponse.Content.ReadAsStringAsync();
    }

    public async Task LogoutUser(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var checkRefreshToken = await CheckRefreshToken(refreshTokenRequestDto);
        
        if (!checkRefreshToken.IsSuccessStatusCode)
            throw new ApplicationException("Invalid refresh token");
        
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "refresh_token", refreshTokenRequestDto.RefreshToken },
            { "client_id", _config["Keycloak:ClientId"] }
        });
        
        var response = await httpClient.PostAsync(_config["Keycloak:LogoutUrl"], requestContent);
        
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Logout failed. Status Code: {response.StatusCode}");
    }

    private async Task<HttpResponseMessage> CheckRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "token", refreshTokenRequestDto.RefreshToken },
            { "client_id", _config["Keycloak:ManageClientId"] },
            { "client_secret", _config["Keycloak:ClientSecret"] }
        });
        
        return await httpClient.PostAsync(_config["Keycloak:ValidationUrl"], requestContent);
    }

    private Credentials CreatePasswordCredentials(string password)
    {
        return new Credentials
        {
            Temporary = false,
            Type = "password",
            Value = password
        };
    }
}