namespace Meetups.Backend.Application.Modules.Auth;

using System;
using Meetups.Backend.Domain.Entities.User;
using Meetups.Contract.Models.Tokens;

public interface ITokenHelper
{
    bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload);

    ITokenPair IssueTokenPair(User user, Guid refreshTokenId);
}