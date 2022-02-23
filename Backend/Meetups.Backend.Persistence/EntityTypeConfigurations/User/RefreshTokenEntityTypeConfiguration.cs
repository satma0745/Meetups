namespace Meetups.Backend.Persistence.EntityTypeConfigurations.User;

using Meetups.Backend.Entities.User;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity)
    {
        refreshTokenEntity.ToTable(RefreshTokenNaming.Table);

        refreshTokenEntity
            .HasKey(refreshToken => refreshToken.TokenId)
            .HasName(RefreshTokenNaming.Indices.PrimaryKey);

        refreshTokenEntity
            .HasIndex(refreshToken => refreshToken.BearerId)
            .HasDatabaseName(RefreshTokenNaming.Indices.BearerId);

        refreshTokenEntity
            .Property(refreshToken => refreshToken.TokenId)
            .HasColumnName(RefreshTokenNaming.Columns.TokenId)
            .ValueGeneratedNever();

        refreshTokenEntity
            .Property(refreshToken => refreshToken.BearerId)
            .HasColumnName(RefreshTokenNaming.Columns.BearerId);
    }
}