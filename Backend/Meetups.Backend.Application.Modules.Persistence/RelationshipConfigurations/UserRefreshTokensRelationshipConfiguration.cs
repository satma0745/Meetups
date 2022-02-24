﻿namespace Meetups.Backend.Application.Modules.Persistence.RelationshipConfigurations;

using Meetups.Backend.Application.Modules.Persistence.Naming;
using Meetups.Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserRefreshTokensRelationshipConfiguration : IRelationshipConfiguration<User, RefreshToken>
{
    public void Configure(EntityTypeBuilder<User> userEntity, EntityTypeBuilder<RefreshToken> refreshTokenEntity)
    {
        refreshTokenEntity
            .HasIndex(refreshToken => refreshToken.BearerId)
            .HasDatabaseName(RefreshTokenNaming.Indices.BearerId);

        userEntity
            .HasMany(user => user.RefreshTokens)
            .WithOne()
            .HasForeignKey(refreshToken => refreshToken.BearerId)
            .HasConstraintName(RefreshTokenNaming.ForeignKeys.BearerId);
    }
}