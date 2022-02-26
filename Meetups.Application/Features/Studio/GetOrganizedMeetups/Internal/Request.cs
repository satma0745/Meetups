namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;

using System;

public class Request
{
    public Guid CurrentUserId { get; }

    public Request(Guid currentUserId) =>
        CurrentUserId = currentUserId;
}