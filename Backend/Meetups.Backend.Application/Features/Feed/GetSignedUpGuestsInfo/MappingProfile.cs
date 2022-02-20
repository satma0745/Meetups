namespace Meetups.Backend.Application.Features.Feed.GetSignedUpGuestsInfo;

using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;
using Meetups.Backend.Entities.User;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Guest, ResponseDto>();
}