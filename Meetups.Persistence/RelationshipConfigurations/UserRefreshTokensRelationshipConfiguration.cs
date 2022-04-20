namespace Meetups.Persistence.RelationshipConfigurations;

using Meetups.Domain.Entities.User;
using Meetups.Persistence.Naming;
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