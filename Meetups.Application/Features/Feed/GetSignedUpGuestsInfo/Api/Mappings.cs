namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Api;

using System.Linq;
using Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult)
    {
        var guestDtos = internalResult.Select(guest => new GuestDto(guest.Id, guest.DisplayName));
        return new ResponseDto(guestDtos);
    }
}