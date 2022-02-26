namespace Meetups.Application.Features.Auth.RegisterNewUser.Api;

using Meetups.Application.Features.Auth.RegisterNewUser.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto) =>
        new(requestDto.Username, requestDto.Password, requestDto.DisplayName, requestDto.AccountType);
}