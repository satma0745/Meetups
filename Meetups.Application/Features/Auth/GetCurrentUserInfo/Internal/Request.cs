namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;

using System;

public record Request(Guid CurrentUserId);