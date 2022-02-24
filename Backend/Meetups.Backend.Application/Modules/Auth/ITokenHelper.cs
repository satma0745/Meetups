namespace Meetups.Backend.Application.Modules.Auth;

using System;
using Meetup.Contract.Models.Tokens;
using Meetups.Backend.Domain.Entities.User;

public interface ITokenHelper
{
    bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload);

    ITokenPair IssueTokenPair(User user, Guid refreshTokenId);
}