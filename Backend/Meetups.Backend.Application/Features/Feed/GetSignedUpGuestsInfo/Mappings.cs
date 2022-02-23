namespace Meetups.Backend.Application.Features.Feed.GetSignedUpGuestsInfo;

using Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;
using Meetups.Backend.Domain.Entities.User;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Guest guest) =>
        new()
        {
            Id = guest.Id,
            DisplayName = guest.DisplayName
        };
}