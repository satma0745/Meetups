namespace Meetup.Contract.Models.Primitives;

using System;
using System.ComponentModel.DataAnnotations;

public class MeetupPlaceDto
{
    /// <summary>Id of the city where the meetup will take place.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid CityId { get; set; }
    
    /// <summary>The address where the meetup will take place.</summary>
    /// <example>Frank street, building 17, floor 7, office 41</example>
    [Required]
    [MaxLength(75)]
    public string Address { get; set; }
}