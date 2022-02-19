namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>()
            .ForMember(
                response => response.SignedUpGuestsCount,
                options => options.MapFrom(meetup => meetup.SignedUpGuests.Count));
}