namespace Meetups.Backend.Application.Features.Studio.UpdateSpecificMeetup;

using AutoMapper;
using Meetup.Contract.Models.Features.Studio.UpdateSpecificMeetup;
using Meetups.Backend.Persistence.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, Meetup>();
}