namespace Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

public enum ErrorTypes
{
    InvalidRefreshTokenProvided,
    TokenBearerDoesNotExist,
    FakeOrUsedRefreshTokenProvided
}