namespace Meetups.Backend.Application.Features.Feed.GetAllCities;

using Meetups.Backend.Domain.Entities.Meetup;
using Meetups.Contract.Models.Features.Feed.GetAllCities;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this City city) =>
        new(city.Id, city.Name);
}