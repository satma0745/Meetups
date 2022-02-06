namespace Meetups.Persistence.RelationshipConfigurations;

using Meetups.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserRefreshTokensRelationshipConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity) =>
        refreshTokenEntity
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_refresh_tokens_user_id");
}