namespace Meetups.Features.MappingProfiles;

using AutoMapper;
using Meetups.Features.DataTransferObjects.User;
using Meetups.Persistence.Entities;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, ReadUserDto>();
        CreateMap<RegisterUserDto, User>()
            .ForMember(user => user.Password, config => config.Ignore());
    }
}
