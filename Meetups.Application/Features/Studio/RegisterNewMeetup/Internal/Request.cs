namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

using System;

public record Request(
    string Topic,
    MeetupPlaceModel Place,
    DateTime StartTime,
    MeetupDurationModel Duration,
    Guid OrganizerId);

public record MeetupPlaceModel(
    Guid CityId,
    string Address);

public record MeetupDurationModel(
    int Hours,
    int Minutes);