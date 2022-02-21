namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserRefreshTokensRelationshipConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity) =>
        refreshTokenEntity
            .HasOne<User>()
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.BearerId)
            .HasConstraintName("fk_users_refresh_tokens_user_id");
}