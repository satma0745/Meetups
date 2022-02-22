namespace Meetups.Backend.Application.Helpers.Tokens;

public class TokenPair
{
    public string AccessToken { get; }
    
    public string RefreshToken { get; }

    public TokenPair(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}