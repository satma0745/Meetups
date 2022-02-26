namespace Meetups.Application.Features.Auth.RefreshTokenPair.Api;

using Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new(internalResult.AccessToken, internalResult.RefreshToken);
}