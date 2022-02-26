namespace Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

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