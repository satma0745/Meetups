namespace Meetups.Application.Features.Auth.GetCurrentUserInfo;

using Meetups.Domain.Entities.User;

internal static class MappingProfile
{
    public static ResponseDto ToResponseDto(this User user) =>
        new(user.Id, user.Username, user.DisplayName);
}