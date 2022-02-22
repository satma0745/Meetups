namespace Meetup.Contract.Models.Features.Studio.RegisterNewCity;

using System;

public class ResponseDto
{
    /// <summary>Registered city ID.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Registered city name.</summary>
    /// <example>Oslo</example>
    public string Name { get; set; }
}