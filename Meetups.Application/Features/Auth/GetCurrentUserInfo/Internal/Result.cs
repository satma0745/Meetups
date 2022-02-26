namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;

using System;

public record Result(
    Guid Id,
    string Username,
    string DisplayName);