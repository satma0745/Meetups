namespace Meetups.Backend.Core.Features.Studio.RegisterNewMeetup;

using AutoMapper;
using Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, Meetup>();
}