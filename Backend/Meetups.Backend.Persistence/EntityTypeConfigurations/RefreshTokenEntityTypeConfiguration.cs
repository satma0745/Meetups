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
            .HasKey(x => x.TokenId)
            .HasName("pk_refresh_tokens");

        refreshTokenEntity
            .HasIndex(x => x.BearerId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        refreshTokenEntity
            .Property(x => x.TokenId)
            .HasColumnName("token_id")
            .ValueGeneratedNever();

        refreshTokenEntity
            .Property(x => x.BearerId)
            .HasColumnName("user_id");
    }
}