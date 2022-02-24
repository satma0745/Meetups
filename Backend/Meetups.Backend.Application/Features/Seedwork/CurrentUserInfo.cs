namespace Meetups.Backend.Application.Features.Seedwork;

using System;

public class CurrentUserInfo
{
    public Guid UserId { get; }

    public CurrentUserInfo(Guid userId) =>
        UserId = userId;
}