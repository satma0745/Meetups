namespace Meetups.Backend.Features.Auth.RegisterNewUser;

using AutoMapper;
using Meetup.Contract.Models.Features.Auth.RegisterNewUser;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RequestDto, Guest>().ForMember(user => user.Password, config => config.Ignore());
        CreateMap<RequestDto, Organizer>().ForMember(user => user.Password, config => config.Ignore());
    }
}