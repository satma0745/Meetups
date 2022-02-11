namespace Meetups.Backend.Features.Feed.GetSignedUpGuestsInfo;

using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Guest, ResponseDto>();
}