namespace Meetups.Backend.Persistence.EntityTypeConfigurations;

using Meetups.Backend.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity)
    {
        refreshTokenEntity.ToTable("refresh_tokens");

        refreshTokenEntity
            .HasKey(refreshToken => refreshToken.TokenId)
            .HasName("pk_refresh_tokens");

        refreshTokenEntity
            .HasIndex(refreshToken => refreshToken.BearerId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        refreshTokenEntity
            .Property(refreshToken => refreshToken.TokenId)
            .HasColumnName("token_id")
            .ValueGeneratedNever();

        refreshTokenEntity
            .Property(refreshToken => refreshToken.BearerId)
            .HasColumnName("user_id");
    }
}