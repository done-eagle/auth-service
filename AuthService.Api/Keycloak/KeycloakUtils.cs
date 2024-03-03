using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using AutoMapper;
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
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var tokenRequestParameters = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", _config["Keycloak:ClientId"] },
                { "code", getAccessTokenRequestDto.AuthCode },
                { "redirect_uri", "https://localhost:8080/redirect" },
                { "code_verifier", getAccessTokenRequestDto.CodeVerifier },
            };

            var tokenResponse = await httpClient.PostAsync(_config["Keycloak:TokenUrl"], 
                new FormUrlEncodedContent(tokenRequestParameters));

            if (!tokenResponse.IsSuccessStatusCode)
                throw new ApplicationException("Access token not received");
            
            return await tokenResponse.Content.ReadAsStringAsync();
        }
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