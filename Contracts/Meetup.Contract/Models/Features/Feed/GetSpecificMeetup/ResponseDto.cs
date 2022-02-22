namespace Meetup.Contract.Models.Features.Feed.GetSpecificMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using Meetup.Contract.Models.Primitives;

public class ResponseDto
{
    /// <summary>Permanent unique meetup identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    public string Topic { get; set; }
    
    public CustomMeetupPlaceDto Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    public MeetupDurationDto Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    public DateTime StartTime { get; set; }
    
    /// <summary>Number of guests that signed up for this meetup.</summary>
    /// <example>42</example>
    public int SignedUpGuestsCount { get; set; }
}

public class CustomMeetupPlaceDto : MeetupPlaceDto
{
    /// <summary>Name of the city where the meetup will take place.</summary>
    /// <example>Oslo</example>
    public string CityName { get; set; }
}