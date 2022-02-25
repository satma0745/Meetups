namespace Meetups.Contract.Models.Features.Studio.RegisterNewCity;

using System;
using JetBrains.Annotations;

public class ResponseDto
{
    /// <summary>Registered city ID.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>Registered city name.</summary>
    /// <example>Oslo</example>
    [PublicAPI]
    public string Name { get; }

    public ResponseDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}