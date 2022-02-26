namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;

using System;

public class Request
{
    public Guid MeetupId { get; }

    public Request(Guid meetupId) =>
        MeetupId = meetupId;
}