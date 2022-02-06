﻿namespace Meetups.Features.Studio.GetOrganizedMeetups;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Meetups.Persistence.Entities;
using Meetups.WebApi.Json;

public class OrganizedMeetupDto
{
    /// <summary>Permanent unique meetup identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    public string Topic { get; set; }
    
    /// <summary>Where meetup takes place.</summary>
    /// <example>Oslo</example>
    public string Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    [JsonConverter(typeof(MeetupDurationJsonConverter))]
    public Meetup.MeetupDuration Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    public DateTime StartTime { get; set; }
    
    /// <summary>Number of guests that signed up for this meetup.</summary>
    /// <example>42</example>
    public int SignedUpGuestsCount { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, OrganizedMeetupDto>()
            .ForMember(
                response => response.SignedUpGuestsCount,
                options => options.MapFrom(meetup => meetup.SignedUpGuests.Count));
}