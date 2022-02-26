namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;

public class ResponseDto
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
    public CustomMeetupPlaceDto Place { get; }
    
    /// <summary>Meetup duration.</summary>
    [PublicAPI]
    [Required]
    public MeetupDurationDto Duration { get; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [PublicAPI]
    public DateTime StartTime { get; }
    
    /// <summary>Number of guests that signed up for this meetup.</summary>
    /// <example>42</example>
    [PublicAPI]
    public int SignedUpGuestsCount { get; }

    public ResponseDto(
        Guid id,
        string topic,
        CustomMeetupPlaceDto place,
        MeetupDurationDto duration,
        DateTime startTime,
        int signedUpGuestsCount)
    {
        Id = id;
        Topic = topic;
        Place = place;
        Duration = duration;
        StartTime = startTime;
        SignedUpGuestsCount = signedUpGuestsCount;
    }
}

public class CustomMeetupPlaceDto : MeetupPlaceDto
{
    /// <summary>Name of the city where the meetup will take place.</summary>
    /// <example>Oslo</example>
    [PublicAPI]
    public string CityName { get; }

    public CustomMeetupPlaceDto(Guid cityId, string cityName, string address)
        : base(cityId, address) =>
        CityName = cityName;
}