namespace Meetups.Application.Seedwork.Api;

using System;

public class CurrentUserInfo
{
    public Guid UserId { get; }

    public CurrentUserInfo(Guid userId) =>
        UserId = userId;
}