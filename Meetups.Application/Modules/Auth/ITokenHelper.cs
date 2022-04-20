namespace Meetups.Application.Modules.Auth;

using System;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Domain.Entities.User;

public interface ITokenHelper
{
    bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload);

    ITokenPair IssueTokenPair(User user, Guid refreshTokenId);
}