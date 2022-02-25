namespace Meetups.Contract.Models.Primitives;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

public class MeetupPlaceDto
{
    /// <summary>Id of the city where the meetup will take place.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid CityId { get; }
    
    /// <summary>The address where the meetup will take place.</summary>
    /// <example>Frank street, building 17, floor 7, office 41</example>
    [PublicAPI]
    [Required]
    [MaxLength(75)]
    public string Address { get; }

    [JsonConstructor]
    public MeetupPlaceDto(Guid cityId, string address)
    {
        CityId = cityId;
        Address = address;
    }
}