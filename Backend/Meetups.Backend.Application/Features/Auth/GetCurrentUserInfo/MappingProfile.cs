namespace Meetups.Backend.Application.Features.Auth.GetCurrentUserInfo;

using AutoMapper;
using Meetup.Contract.Models.Features.Auth.GetCurrentUserInfo;
using Meetups.Backend.Entities.User;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<User, ResponseDto>();
}