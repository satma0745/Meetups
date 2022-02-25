namespace Meetups.Application.Features.Feed.GetAllCities;

using Meetups.Domain.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this City city) =>
        new(city.Id, city.Name);
}