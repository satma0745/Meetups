namespace Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

public class Request
{
    public string OldRefreshToken { get; }

    public Request(string oldRefreshToken) =>
        OldRefreshToken = oldRefreshToken;
}