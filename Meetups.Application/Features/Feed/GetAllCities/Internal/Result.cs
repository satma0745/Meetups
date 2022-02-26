namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

using System;
using System.Collections.Generic;

public class Result : List<CityModel>
{
    public Result(IEnumerable<CityModel> cities)
        : base(cities)
    {
    }
}

public record CityModel(
    Guid Id,
    string Name);