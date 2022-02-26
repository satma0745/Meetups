namespace Meetups.Application.Features.Auth.AuthenticateUser.Api;

using Meetups.Application.Features.Auth.AuthenticateUser.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto) =>
        new(requestDto.Username, requestDto.Password);

    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new(internalResult.AccessToken, internalResult.RefreshToken);
}