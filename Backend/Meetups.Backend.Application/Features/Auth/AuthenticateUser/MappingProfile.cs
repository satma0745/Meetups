namespace Meetups.Backend.Application.Features.Auth.AuthenticateUser;

using AutoMapper;
using Meetup.Contract.Models.Features.Auth.AuthenticateUser;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, User>()
            .ForMember(user => user.Password, config => config.Ignore());
}