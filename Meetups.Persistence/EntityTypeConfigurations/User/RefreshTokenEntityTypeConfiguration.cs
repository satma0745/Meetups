namespace Meetups.Persistence.EntityTypeConfigurations.User;

using Meetups.Domain.Entities.User;
using Meetups.Persistence.Naming;
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
            .Property(refreshToken => refreshToken.TokenId)
            .HasColumnName(RefreshTokenNaming.Columns.TokenId)
            .ValueGeneratedNever();

        refreshTokenEntity
            .Property(refreshToken => refreshToken.BearerId)
            .HasColumnName(RefreshTokenNaming.Columns.BearerId);
    }
}