namespace Meetups.Backend.Application.Features.Feed.GetSignedUpGuestsInfo;

using Meetups.Backend.Domain.Entities.User;
using Meetups.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Guest guest) =>
        new(guest.Id, guest.DisplayName);
}