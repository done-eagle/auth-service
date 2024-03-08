using AuthService.Api.Keycloak;
using AuthService.Api.Mapper;
using AuthService.Api.Validators;
using AuthService.Api.Сonverters;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FindUserByIdDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetAccessTokenDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddLogging(logging =>
{
    logging.AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug);
});

// KeyCloak
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwtOptions =>
{
    jwtOptions.Authority = builder.Configuration["Keycloak:Authority"];
    jwtOptions.Audience = builder.Configuration["Keycloak:Audience"];
    jwtOptions.RequireHttpsMetadata = false;
    jwtOptions.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context => Task.CompletedTask
    };
});

builder.Services.AddSingleton<IClaimsTransformation, KcRoleConverter>();

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IKeycloakUtils>(provider =>
{
    var mapper = provider.GetRequiredService<IMapper>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

    return new KeycloakUtils(
        configuration["Keycloak:ServerUrl"],
        configuration["Keycloak:ManageClientId"],
        configuration["Keycloak:ClientSecret"],
        mapper,
        configuration,
        httpClientFactory
    );
});
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
