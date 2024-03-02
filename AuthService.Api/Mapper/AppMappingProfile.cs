using AuthService.Api.Dto.Request;
using AuthService.Api.Dto.Response;
using AutoMapper;
using Keycloak.Net.Models.Users;

namespace AuthService.Api.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, FindUserByIdResponseDto>();
        CreateMap<CreateUserRequestDto, User>();
    }
}