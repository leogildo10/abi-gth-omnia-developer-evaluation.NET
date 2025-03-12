using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using DTO = Ambev.DeveloperEvaluation.Application.DTOs;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.Application.Commands.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using GetUserResponse = Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser.GetUserResponse;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, AuthenticateUserResponse>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
        CreateMap<DTO.Auth.AuthenticateUser.AuthenticateUserResponse, AuthenticateUserResponse>();

        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<User, CreateUserCommand>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, DTO.Users.CreateUser.CreateUserResponse>();
        CreateMap<DTO.Users.CreateUser.CreateUserResponse, CreateUserResponse>();

        CreateMap<Guid, DeleteUserCommand>()
            .ConstructUsing(id => new DeleteUserCommand(id));

        CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));
        CreateMap<User, DTO.Users.GetUser.GetUserResponse>();
        CreateMap<DTO.Users.GetUser.GetUserResponse, GetUserResponse>();
        CreateMap<DTO.Users.GetUser.GetUserResponse, ListUsersResponse>();

        CreateMap<User, ListUsersResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<ListUsersRequest, ListUsersCommand>();

        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<User, DTO.Users.UpdateUser.UpdateUserResponse>();
        CreateMap<DTO.Users.UpdateUser.UpdateUserResponse, UpdateUserResponse>();
    }
}