namespace Meetups.Application.Features.Auth.AuthenticateUser.Internal;

public class Result
{
    public string AccessToken { get; }
    
    public string RefreshToken { get; }

    public Result(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}