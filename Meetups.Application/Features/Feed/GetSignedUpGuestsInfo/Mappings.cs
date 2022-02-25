namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo;

using Meetups.Domain.Entities.User;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Guest guest) =>
        new(guest.Id, guest.DisplayName);
}