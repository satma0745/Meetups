namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;

using System;

public record Result(
    Guid Id,
    string Topic,
    MeetupPlaceModel Place,
    DateTime StartTime,
    MeetupDurationModel Duration,
    int SignedUpGuestsCount);

public record MeetupPlaceModel(
    Guid CityId,
    string CityName,
    string Address);

public record MeetupDurationModel(
    int Hours,
    int Minutes);