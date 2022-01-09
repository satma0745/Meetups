namespace Meetups.MappingProfiles;

using System;
using AutoMapper;
using Meetups.DataTransferObjects;
using Meetups.Entities;

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
