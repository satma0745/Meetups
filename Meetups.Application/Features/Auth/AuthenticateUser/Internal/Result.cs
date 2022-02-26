namespace Meetups.Application.Features.Auth.AuthenticateUser.Internal;

public record Result(
    string AccessToken,
    string RefreshToken);