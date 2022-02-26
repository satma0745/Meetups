namespace Meetups.Application.Features.Auth.AuthenticateUser.Api;

using JetBrains.Annotations;

public class ResponseDto
{
    /// <summary>Short-living token used for user authorization.</summary>
    [PublicAPI]
    public string AccessToken { get; }

    /// <summary>Long-living persisted token used to obtain new access tokens.</summary>
    [PublicAPI]
    public string RefreshToken { get; }

    public ResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}