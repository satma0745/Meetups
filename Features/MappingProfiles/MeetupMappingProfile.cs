namespace Meetups.Features.MappingProfiles;

using System;
using AutoMapper;
using Meetups.Features.DataTransferObjects.Meetup;
using Meetups.Persistence.Entities;

internal class MeetupMappingProfile : Profile
{
    public MeetupMappingProfile()
    {
        CreateMap<Meetup, ReadMeetupDto>();
        CreateMap<WriteMeetupDto, Meetup>()
            .ForMember(
                meetup => meetup.Duration,
                config => config.MapFrom(writeDto => TimeSpan.FromMinutes(writeDto.Duration)));
    }
}
