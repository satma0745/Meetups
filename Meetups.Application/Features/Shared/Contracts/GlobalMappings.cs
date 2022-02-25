namespace Meetups.Application.Features.Shared.Contracts;

using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;
using Meetups.Application.Modules.Auth;
using Meetups.Domain.Entities.Meetup;

internal static class GlobalMappings
{
    #region MeetupDuration

    public static MeetupDuration ToMeetupDuration(this MeetupDurationDto meetupDurationDto) =>
        new(meetupDurationDto.Hours, meetupDurationDto.Minutes);

    public static MeetupDurationDto ToMeetupDurationDto(this MeetupDuration meetupDuration) =>
        new(meetupDuration.Hours, meetupDuration.Minutes);

    #endregion
    
    #region MeetupPlace

    public static MeetupPlace ToMeetupPlace(this MeetupPlaceDto meetupPlaceDto, City city) =>
        new(city, meetupPlaceDto.Address);

    public static MeetupPlaceDto ToMeetupPlaceDto(this MeetupPlace meetupPlace) =>
        new(meetupPlace.City.Id, meetupPlace.Address);

    #endregion
    
    #region ITokenPair

    public static TokenPairDto ToTokenPairDto(this ITokenPair tokenPair) =>
        new(tokenPair.AccessToken, tokenPair.RefreshToken);

    #endregion
}