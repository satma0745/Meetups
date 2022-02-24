namespace Meetups.Backend.Application.Modules.Persistence.EntityTypeConfigurations.User;

using Meetups.Backend.Application.Modules.Persistence.Naming;
using Meetups.Backend.Domain.Entities.User;
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