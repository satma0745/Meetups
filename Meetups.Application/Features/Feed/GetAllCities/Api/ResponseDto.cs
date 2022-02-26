namespace Meetups.Application.Features.Feed.GetAllCities.Api;

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class ResponseDto : List<CityDto>
{
    public ResponseDto(IEnumerable<CityDto> cities)
        : base(cities)
    {
    }
}

public class CityDto
{
    /// <summary>City ID.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>City name.</summary>
    /// <example>Oslo</example>
    [PublicAPI]
    public string Name { get; }

    public CityDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}