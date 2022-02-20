namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserRefreshTokensRelationshipConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity) =>
        refreshTokenEntity
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.BearerId)
            .HasConstraintName("fk_users_refresh_tokens_user_id");
}