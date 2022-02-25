namespace Meetups.Application.Features.Shared.Infrastructure;

using System;

public class CurrentUserInfo
{
    public Guid UserId { get; }

    public CurrentUserInfo(Guid userId) =>
        UserId = userId;
}