namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;

using System;

public class Result
{
    public Guid Id { get; }
    
    public string Username { get; }
    
    public string DisplayName { get; }

    public Result(Guid id, string username, string displayName)
    {
        Id = id;
        Username = username;
        DisplayName = displayName;
    }
}