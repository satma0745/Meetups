namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Entities.User;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserRefreshTokensRelationshipConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity) =>
        refreshTokenEntity
            .HasOne<User>()
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.BearerId)
            .HasConstraintName(RefreshTokenNaming.ForeignKeys.BearerId);
}