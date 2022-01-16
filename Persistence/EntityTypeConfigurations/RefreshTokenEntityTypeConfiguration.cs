namespace Meetups.Persistence.EntityTypeConfigurations;

using Meetups.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity)
    {
        refreshTokenEntity.ToTable("refresh_tokens");

        refreshTokenEntity
            .HasKey(x => x.TokenId)
            .HasName("pk_refresh_tokens");

        refreshTokenEntity
            .HasIndex(x => x.UserId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        refreshTokenEntity
            .Property(x => x.TokenId)
            .HasColumnName("token_id");

        refreshTokenEntity
            .Property(x => x.UserId)
            .HasColumnName("user_id");

        refreshTokenEntity
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_refresh_tokens_user_id");
    }
}