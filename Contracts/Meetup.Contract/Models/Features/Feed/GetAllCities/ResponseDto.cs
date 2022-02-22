namespace Meetup.Contract.Models.Features.Feed.GetAllCities;

using System;

public class ResponseDto
{
    /// <summary>City ID.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>City name.</summary>
    /// <example>Oslo</example>
    public string Name { get; set; }
}