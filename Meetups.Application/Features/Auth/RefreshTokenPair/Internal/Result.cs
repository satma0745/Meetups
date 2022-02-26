namespace Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

public record Result(
    string AccessToken,
    string RefreshToken);