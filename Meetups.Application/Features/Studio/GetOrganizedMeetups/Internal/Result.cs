namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;

using System;
using System.Collections.Generic;

public class Result : List<MeetupModel>
{
    public Result(IEnumerable<MeetupModel> meetups)
        : base(meetups)
    {
    }
}

public record MeetupModel(
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