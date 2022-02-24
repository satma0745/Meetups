namespace Meetups.Backend.Application.Modules.Auth.TokenHelper;

using Meetups.Backend.Application.Modules.Auth;

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