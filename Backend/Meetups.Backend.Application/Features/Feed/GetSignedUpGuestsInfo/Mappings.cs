namespace Meetups.Backend.Application.Features.Feed.GetSignedUpGuestsInfo;

using Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;
using Meetups.Backend.Domain.Entities.User;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Guest guest) =>
        new(guest.Id, guest.DisplayName);
}