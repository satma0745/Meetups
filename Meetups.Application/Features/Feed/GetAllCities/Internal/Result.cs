namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

using System.Collections.Generic;
using Meetups.Domain.Entities.Meetup;

public class Result : List<City>
{
    public Result(IEnumerable<City> cities)
        : base(cities)
    {
    }
}