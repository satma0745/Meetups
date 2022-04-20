namespace Meetups.Auth.TokenHelper;

using Meetups.Application.Modules.Auth;

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
