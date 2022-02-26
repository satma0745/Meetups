namespace Meetups.Application.Features.Auth.SignOutEverywhere.Internal;

using System;

public class Request
{
    public Guid CurrentUserId { get; }

    public Request(Guid currentUserId) =>
        CurrentUserId = currentUserId;
}