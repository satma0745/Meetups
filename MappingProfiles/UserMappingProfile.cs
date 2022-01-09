namespace Meetups.MappingProfiles;

using AutoMapper;
using Meetups.DataTransferObjects.User;
using Meetups.Entities;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, ReadUserDto>();
        CreateMap<WriteUserDto, User>();
    }
}
