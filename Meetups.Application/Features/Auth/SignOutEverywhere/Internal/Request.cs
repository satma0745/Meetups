namespace Meetups.Application.Features.Auth.SignOutEverywhere.Internal;

using System;

public record Request(Guid CurrentUserId);