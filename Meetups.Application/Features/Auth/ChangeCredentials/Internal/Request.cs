namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

using System;

public record Request(
    Guid UserId,
    string Username,
    string Password);