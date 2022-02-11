namespace Meetups.Backend.Features.Shared;

using AutoMapper;
using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup.MeetupDuration, MeetupDurationDto>();
}