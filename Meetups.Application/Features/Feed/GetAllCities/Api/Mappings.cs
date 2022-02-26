namespace Meetups.Application.Features.Feed.GetAllCities.Api;

using System.Linq;
using Meetups.Application.Features.Feed.GetAllCities.Internal;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult)
    {
        var cityDtos = internalResult.Select(city => new CityDto(city.Id, city.Name));
        return new ResponseDto(cityDtos);
    }
}