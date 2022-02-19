namespace Meetups.Backend.Application.Features.Studio.GetOrganizedMeetups;

using AutoMapper;
using Meetup.Contract.Models.Features.Studio.GetOrganizedMeetups;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>()
            .ForMember(
                response => response.SignedUpGuestsCount,
                options => options.MapFrom(meetup => meetup.SignedUpGuests.Count));
}