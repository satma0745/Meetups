namespace Meetups.Backend.WebApi.Mapping;

using AutoMapper;
using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Entities.Meetup;

internal class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile() =>
        CreateMap<MeetupDuration, MeetupDurationDto>();
}