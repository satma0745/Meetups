namespace Meetups.Backend.Core.Features.Auth.GetCurrentUserInfo;

using AutoMapper;
using Meetup.Contract.Models.Features.Auth.GetCurrentUserInfo;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<User, ResponseDto>();
}