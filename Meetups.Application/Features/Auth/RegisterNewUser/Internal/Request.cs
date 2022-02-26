namespace Meetups.Application.Features.Auth.RegisterNewUser.Internal;

public class Request
{
    public string Username { get; }
    
    public string Password { get; }
    
    public string DisplayName { get; }
    
    public string AccountType { get; }

    public Request(string username, string password, string displayName, string accountType)
    {
        Username = username;
        Password = password;
        DisplayName = displayName;
        AccountType = accountType;
    }
}