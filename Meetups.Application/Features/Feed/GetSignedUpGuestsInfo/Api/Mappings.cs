namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Api;

using System.Collections.Generic;
using System.Linq;
using Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;

internal static class Mappings
{
    public static IEnumerable<ResponseDto> ToApiResponse(this Result internalResult) =>
        internalResult.Select(guest => new ResponseDto(guest.Id, guest.DisplayName));
}