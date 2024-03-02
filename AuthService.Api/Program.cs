using AuthService.Api.Keycloak;
using AuthService.Api.Mapper;
using AuthService.Api.Сonverters;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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

builder.Services.AddSingleton<IKeycloakUtils>(provider =>
{
    var mapper = provider.GetRequiredService<IMapper>();

    return new KeycloakUtils(
        builder.Configuration["Keycloak:ServerUrl"],
        builder.Configuration["Keycloak:ClientId"],
        builder.Configuration["Keycloak:ClientSecret"],
        mapper
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
