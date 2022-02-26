namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Api;

using Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;

internal static class MappingProfile
{
    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new(internalResult.Id, internalResult.Username, internalResult.DisplayName);
}