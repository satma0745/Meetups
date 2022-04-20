namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Meetups.Application.Features.Shared.PrimitiveDtos;

public class ResponseDto : List<MeetupDto>
{
    public ResponseDto(IEnumerable<MeetupDto> meetups)
        : base(meetups)
    {
    }
}

public class MeetupDto
{
    /// <summary>Permanent unique meetup identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [PublicAPI]
    public string Topic { get; }
    
    [PublicAPI]
    public MeetupPlaceDto Place { get; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [PublicAPI]
    public DateTime StartTime { get; }
    
    /// <summary>Meetup duration.</summary>
    [PublicAPI]
    [Required]
    public MeetupDurationDto Duration { get; }
    
    /// <summary>Number of guests that signed up for this meetup.</summary>
    /// <example>42</example>
    [PublicAPI]
    public int SignedUpGuestsCount { get; }

    public MeetupDto(
        Guid id,
        string topic,
        MeetupPlaceDto place,
        DateTime startTime,
        MeetupDurationDto duration,
        int signedUpGuestsCount)
    {
        Id = id;
        Topic = topic;
        Place = place;
        StartTime = startTime;
        Duration = duration;
        SignedUpGuestsCount = signedUpGuestsCount;
    }
}

public class MeetupPlaceDto
{
    /// <summary>Id of the city where the meetup will take place.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid CityId { get; }
    
    /// <summary>Name of the city where the meetup will take place.</summary>
    /// <example>Oslo</example>
    [PublicAPI]
    public string CityName { get; }
    
    /// <summary>The address where the meetup will take place.</summary>
    /// <example>Frank street, building 17, floor 7, office 41</example>
    [PublicAPI]
    public string Address { get; }

    public MeetupPlaceDto(Guid cityId, string cityName, string address)
    {
        CityId = cityId;
        CityName = cityName;
        Address = address;
    }
}