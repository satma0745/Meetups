namespace Meetups.Backend.Application.Modules.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Tokens;
using Meetups.Backend.Domain.Entities.User;
using Microsoft.IdentityModel.Tokens;

internal class TokenHelper : ITokenHelper
{
    private readonly AuthConfiguration configuration;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public TokenHelper(AuthConfiguration configuration)
    {
        this.configuration = configuration;

        // Fixes JWT Claims names (by default Microsoft replaces them with links leading to nowhere) 
        tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
    }

    public bool TryParseToken(string token, out IDictionary<string, string> payload)
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

    public TokenPair IssueTokenPair(User user, Guid refreshTokenId) =>
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

    private TokenPair IssueTokenPair(Guid userId, string userRole, Guid refreshTokenId)
    {
        var accessToken = IssueToken(
            new Dictionary<string, object>
            {
                {AccessTokenPayload.UserIdClaim, userId},
                {AccessTokenPayload.UserRoleClaim, userRole}
            },
            configuration.AccessTokenLifetime);

        var refreshToken = IssueToken(
            new Dictionary<string, object>
            {
                {RefreshTokenPayload.UserIdClaim, userId},
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
}