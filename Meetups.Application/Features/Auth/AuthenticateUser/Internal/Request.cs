namespace Meetups.Application.Features.Auth.AuthenticateUser.Internal;

public class Request
{
    public string Username { get; }
    public string Password { get; }

    public Request(string username, string password)
    {
        Username = username;
        Password = password;
    }
}