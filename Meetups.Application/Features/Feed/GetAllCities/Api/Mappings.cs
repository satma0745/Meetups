namespace Meetups.Application.Features.Feed.GetAllCities.Api;

using System.Collections.Generic;
using System.Linq;
using Meetups.Application.Features.Feed.GetAllCities.Internal;

internal static class Mappings
{
    public static IEnumerable<ResponseDto> ToApiResponse(this Result internalResult) =>
        internalResult.Select(city => new ResponseDto(city.Id, city.Name));
}