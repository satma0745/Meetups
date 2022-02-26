namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

using System;

public record Request(
    Guid CurrentUserId,
    string Username,
    string Password);