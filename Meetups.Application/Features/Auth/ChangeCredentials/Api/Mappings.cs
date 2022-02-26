namespace Meetups.Application.Features.Auth.ChangeCredentials.Api;

using Meetups.Application.Features.Auth.ChangeCredentials.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, CurrentUserInfo currentUserInfo) =>
        new(currentUserInfo.UserId, requestDto.Username, requestDto.Password);
}