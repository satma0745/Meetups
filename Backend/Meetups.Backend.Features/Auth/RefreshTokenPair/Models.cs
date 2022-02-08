namespace Meetups.Backend.Features.Auth.RefreshTokenPair;

public class ResponseDto
{
    /// <summary>Short-living token used for user authorization.</summary>
    public string AccessToken { get; }

    /// <summary>Long-living persisted token used to obtain new access tokens.</summary>
    public string RefreshToken { get; }

    public ResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}