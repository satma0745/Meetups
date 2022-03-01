namespace Meetups.Tests.Mocks;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Modules.Auth;
using Meetups.Domain.Entities.User;

internal class TokenHelperMock : ITokenHelper
{
    public bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload)
    {
        try
        {
            var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(refreshToken);

            var tokenId = Guid.Parse(payload![RefreshTokenPayload.TokenIdClaim]);
            var userId = Guid.Parse(payload[RefreshTokenPayload.BearerIdClaim]);

            refreshTokenPayload = new RefreshTokenPayload(
                tokenId: tokenId,
                bearerId: userId);
            return true;
        }
        catch
        {
            refreshTokenPayload = new RefreshTokenPayload(
                tokenId: Guid.Empty,
                bearerId: Guid.Empty);
            return false;
        }
    }

    public ITokenPair IssueTokenPair(User user, Guid refreshTokenId)
    {
        var accessTokenPayload = new Dictionary<string, string>
        {
            {AccessTokenPayload.BearerIdClaim, user.Id.ToString()},
            {AccessTokenPayload.UserRoleClaim, GetUserRole(user)}
        };
        var accessToken = JsonSerializer.Serialize(accessTokenPayload);
        
        var refreshTokenPayload = new Dictionary<string, string>
        {
            {RefreshTokenPayload.BearerIdClaim, user.Id.ToString()},
            {RefreshTokenPayload.TokenIdClaim, refreshTokenId.ToString()}
        };
        var refreshToken = JsonSerializer.Serialize(refreshTokenPayload);

        return new TokenPair(accessToken, refreshToken);
    }

    private static string GetUserRole(User user) =>
        user switch
        {
            Guest => UserRoles.Guest,
            Organizer => UserRoles.Organizer,
            var unmatched => throw new SwitchExpressionException(unmatched)
        };
    
    private record TokenPair(string AccessToken, string RefreshToken) : ITokenPair;
}