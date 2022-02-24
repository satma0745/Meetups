﻿namespace Meetups.Backend.Application.Features.Auth.GetCurrentUserInfo;

using Meetup.Contract.Models.Features.Auth.GetCurrentUserInfo;
using Meetups.Backend.Domain.Entities.User;

internal static class MappingProfile
{
    public static ResponseDto ToResponseDto(this User user) =>
        new(user.Id, user.Username, user.DisplayName);
}