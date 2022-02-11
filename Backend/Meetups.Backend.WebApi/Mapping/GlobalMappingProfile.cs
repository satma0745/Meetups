namespace Meetups.Backend.WebApi.Mapping;

using AutoMapper;
using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Persistence.Entities;

internal class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile() =>
        CreateMap<Meetup.MeetupDuration, MeetupDurationDto>();
}