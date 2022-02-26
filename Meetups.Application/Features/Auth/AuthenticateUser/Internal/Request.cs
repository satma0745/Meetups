namespace Meetups.Application.Features.Auth.AuthenticateUser.Internal;

public record Request(
    string Username,
    string Password);