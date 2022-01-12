﻿namespace Meetups.Features.Shared;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class TokenHelper
{
    private readonly SigningCredentials signingCredentials;
    private readonly JwtSecurityTokenHandler tokenHandler;
    private readonly TimeSpan accessTokenLifetime;
    private readonly TimeSpan refreshTokenLifetime;
    
    public TokenHelper(IConfiguration configuration)
    {
        // This key is used to sign tokens so that no one can tamper with them.
        var rawSigningKey = configuration["Auth:SecretKey"];
        var signingKeyBytes = Encoding.ASCII.GetBytes(rawSigningKey);
        var signingKey = new SymmetricSecurityKey(signingKeyBytes);
        signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);

        // Fixes JWT Claims names (by default Microsoft replaces them with links leading to nowhere) 
        tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
        
        var accessTokenLifetimeInMinutes = int.Parse(configuration["Auth:AccessTokenLifetimeInMinutes"]);
        accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

        var refreshTokenLifetimeInDays = int.Parse(configuration["Auth:RefreshTokenLifetimeInDays"]);
        refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);
    }

    public (string AccessToken, string RefreshToken) IssueTokenPair(Guid userId, Guid refreshTokenId)
    {
        var accessToken = IssueToken(new Dictionary<string, object> {{"sub", userId}}, accessTokenLifetime);
        var refreshToken = IssueToken(new Dictionary<string, object>
        {
            {"sub", userId},
            {"jti", refreshTokenId}
        }, refreshTokenLifetime);
        return (accessToken, refreshToken);
    }

    public bool TryParseToken(string token, out IDictionary<string, string> payload)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                
                ValidateAudience = false,
                ValidateIssuer = false,
                
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            var claimsInfo = tokenHandler.ValidateToken(token, validationParameters, out _);
            payload = claimsInfo.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

            return true;
        }
        catch (Exception)
        {
            payload = null;
            return false;
        }
    }
    
    private string IssueToken(IDictionary<string, object> payload, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = payload,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = signingCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}