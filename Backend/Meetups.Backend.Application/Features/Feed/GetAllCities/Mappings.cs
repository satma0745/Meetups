namespace Meetups.Backend.Application.Features.Feed.GetAllCities;

using Meetup.Contract.Models.Features.Feed.GetAllCities;
using Meetups.Backend.Domain.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this City city) =>
        new()
        {
            Id = city.Id,
            Name = city.Name
        };
}