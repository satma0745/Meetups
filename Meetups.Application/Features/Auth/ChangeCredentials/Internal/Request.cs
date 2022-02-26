namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

using System;

public class Request
{
    public Guid CurrentUserId { get; }
    
    public string Username { get; }
    
    public string Password { get; }

    public Request(Guid currentUserId, string username, string password)
    {
        CurrentUserId = currentUserId;
        Username = username;
        Password = password;
    }
}