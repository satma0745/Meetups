namespace Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

using System;

public record Request(
    Guid MeetupId,
    MeetupPlaceModel Place,
    DateTime StartTime,
    MeetupDurationModel Duration,
    Guid CurrentUserId);

public record MeetupPlaceModel(
    Guid CityId,
    string Address);

public record MeetupDurationModel(
    int Hours,
    int Minutes);