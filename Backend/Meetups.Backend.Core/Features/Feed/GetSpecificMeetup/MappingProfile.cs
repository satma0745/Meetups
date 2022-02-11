namespace Meetups.Backend.Core.Features.Feed.GetSpecificMeetup;

using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetSpecificMeetup;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>()
            .ForMember(
                response => response.SignedUpGuestsCount,
                options => options.MapFrom(meetup => meetup.SignedUpGuests.Count));
}