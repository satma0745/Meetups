namespace Meetups.Application.Modules.Auth.TokenHelper;

internal class TokenPair : ITokenPair
{
    public string AccessToken { get; }
    
    public string RefreshToken { get; }

    public TokenPair(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}