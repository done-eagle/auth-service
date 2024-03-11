using AuthService.Api.Data;
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

    public async Task<CreateUserResponseDto> CreateUser(CreateUserRequestDto createUserRequestDto)
    {
        try
        {
            var credential = CreatePasswordCredentials(createUserRequestDto.Password);
            var user = _mapper.Map<User>(createUserRequestDto);
        
            user.Enabled = true;
            user.EmailVerified = false;
            user.Credentials = new[] { credential };
        
            var createdUserId = await _keycloakClient.CreateAndRetrieveUserIdAsync(_config[RealmConfigKey], user);
            Console.WriteLine($"User created with userId: {createdUserId}");

            return new CreateUserResponseDto(CodesData.CreatedCode, createdUserId);
        }
        catch (FlurlHttpException ex)
        {
            return new CreateUserResponseDto((int)ex.StatusCode!, "");
        }
    }

    public async Task<FindUserByIdResponseDto> FindById(FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            var user = await _keycloakClient.GetUserAsync(_config[RealmConfigKey], findUserByIdRequestDto.UserId);
            var userDto = _mapper.Map<UserResponseDto>(user);
            return new FindUserByIdResponseDto(CodesData.SuccessCode, userDto);
        }
        catch (FlurlHttpException ex)
        {
            return new FindUserByIdResponseDto((int)ex.StatusCode!, null!);
        }
    }

    public async Task<KeycloakResponseDto> UpdateUser(UpdateUserRequestDto updateUserRequestDto)
    {
        try
        {
            var user = await _keycloakClient.GetUserAsync(_config[RealmConfigKey], updateUserRequestDto.UserId);
            var credential = CreatePasswordCredentials(updateUserRequestDto.Password);
        
            user.Email = updateUserRequestDto.Email;
            user.Credentials = new[] { credential };
        
            await _keycloakClient.UpdateUserAsync(_config[RealmConfigKey], updateUserRequestDto.UserId, user);
            return new KeycloakResponseDto(CodesData.SuccessCode);
        }
        catch (FlurlHttpException ex)
        {
            return new KeycloakResponseDto((int)ex.StatusCode!);
        }
    }

    public async Task<KeycloakResponseDto> DeleteUser(FindUserByIdRequestDto findUserByIdRequestDto)
    {
        try
        {
            await _keycloakClient.DeleteUserAsync(_config[RealmConfigKey], findUserByIdRequestDto.UserId);
            return new KeycloakResponseDto(CodesData.SuccessCode);
        }
        catch (FlurlHttpException ex)
        {
            return new KeycloakResponseDto((int)ex.StatusCode!);
        }
    }
    
    public async Task<GetAccessTokenResponseDto> GetAccessToken(GetAccessTokenRequestDto getAccessTokenRequestDto)
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

        var response = await httpClient.PostAsync(_config["Keycloak:TokenUrl"], 
            new FormUrlEncodedContent(requestContent));

        return new GetAccessTokenResponseDto((int)response.StatusCode, 
            await response.Content.ReadAsStringAsync());
    }

    public async Task<GetAccessTokenResponseDto> GetAccessTokenByRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var checkRefreshTokenResponse = await CheckRefreshToken(refreshTokenRequestDto);

        if (!checkRefreshTokenResponse.IsSuccessStatusCode)
            return new GetAccessTokenResponseDto((int)checkRefreshTokenResponse.StatusCode, "");
        
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", _config["Keycloak:ClientId"] },
            { "refresh_token", refreshTokenRequestDto.RefreshToken }
        });
        
        var response = await httpClient.PostAsync(_config["Keycloak:TokenUrl"], requestContent);

        return new GetAccessTokenResponseDto((int)response.StatusCode,
            await response.Content.ReadAsStringAsync());
    }

    public async Task<KeycloakResponseDto> LogoutUser(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var checkRefreshTokenResponse = await CheckRefreshToken(refreshTokenRequestDto);
        
        if (!checkRefreshTokenResponse.IsSuccessStatusCode)
            return new KeycloakResponseDto((int)checkRefreshTokenResponse.StatusCode);
        
        using var httpClient = _httpClientFactory.CreateClient();
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "refresh_token", refreshTokenRequestDto.RefreshToken },
            { "client_id", _config["Keycloak:ClientId"] }
        });
        
        var response = await httpClient.PostAsync(_config["Keycloak:LogoutUrl"], requestContent);

        return new KeycloakResponseDto((int)response.StatusCode);
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