namespace Meetups.Application.Features.Studio.RegisterNewCity.Api;

using Meetups.Application.Features.Studio.RegisterNewCity.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto) =>
        new(requestDto.Name);

    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new(internalResult.Id, internalResult.Name);
}