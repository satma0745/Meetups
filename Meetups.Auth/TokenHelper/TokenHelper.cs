namespace Meetups.Auth.TokenHelper;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Modules.Auth;
using Meetups.Domain.Entities.User;
using Microsoft.IdentityModel.Tokens;

internal class TokenHelper : ITokenHelper
{
    #region Dependencies & initialization
    
    private readonly IAuthConfiguration configuration;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public TokenHelper(IAuthConfiguration configuration)
    {
        this.configuration = configuration;

        // Fixes JWT Claims names (by default Microsoft replaces them with links leading to nowhere) 
        tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
    }
    
    #endregion

    #region Parsing

    public bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload)
    {
        var success = TryParseToken(refreshToken, out var claims);
        if (!success)
        {
            refreshTokenPayload = null;
            return false;
        }

        var refreshTokenId = Guid.Parse(claims[RefreshTokenPayload.TokenIdClaim]);
        var currentUserId = Guid.Parse(claims[RefreshTokenPayload.BearerIdClaim]);

        refreshTokenPayload = new RefreshTokenPayload(refreshTokenId, currentUserId);
        return true;
    }

    private bool TryParseToken(string token, out IDictionary<string, string> payload)
    {
        try
        {
            var claimsInfo = tokenHandler.ValidateToken(token, configuration.TokenValidationParameters, out _);
            payload = claimsInfo.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            return true;
        }
        catch (Exception)
        {
            payload = null;
            return false;
        }
    }
    
    #endregion

    #region Issuing
    
    public ITokenPair IssueTokenPair(User user, Guid refreshTokenId) =>
        user is null
            ? throw new ArgumentNullException(nameof(user))
            : IssueTokenPair(user.Id, GetUserRole(user), refreshTokenId);

    private static string GetUserRole(User user) =>
        user switch
        {
            Guest => UserRoles.Guest,
            Organizer => UserRoles.Organizer,
            var unmatched => throw new SwitchExpressionException(unmatched)
        };

    private ITokenPair IssueTokenPair(Guid userId, string userRole, Guid refreshTokenId)
    {
        var accessToken = IssueToken(
            new Dictionary<string, object>
            {
                {AccessTokenPayload.BearerIdClaim, userId},
                {AccessTokenPayload.UserRoleClaim, userRole}
            },
            configuration.AccessTokenLifetime);

        var refreshToken = IssueToken(
            new Dictionary<string, object>
            {
                {RefreshTokenPayload.BearerIdClaim, userId},
                {RefreshTokenPayload.TokenIdClaim, refreshTokenId}
            },
            configuration.RefreshTokenLifetime);

        return new TokenPair(accessToken, refreshToken);
    }
    
    private string IssueToken(IDictionary<string, object> payload, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = payload,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = configuration.SigningCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    #endregion
}
