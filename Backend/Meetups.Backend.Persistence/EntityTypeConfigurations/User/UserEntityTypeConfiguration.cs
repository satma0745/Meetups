namespace Meetups.Backend.Persistence.EntityTypeConfigurations.User;

using Meetups.Backend.Domain.Entities.User;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userEntity)
    {
        userEntity.ToTable(UserNaming.Table);

        userEntity
            .HasDiscriminator<string>(UserNaming.Columns.Discriminator)
            .HasValue<Guest>(nameof(Guest))
            .HasValue<Organizer>(nameof(Organizer))
            .HasValue(nameof(Guest));

        userEntity
            .HasKey(user => user.Id)
            .HasName(UserNaming.Indices.PrimaryKey);

        userEntity
            .HasIndex(user => user.Username)
            .IsUnique()
            .HasDatabaseName(UserNaming.Indices.UniqueUsername);

        userEntity
            .Property(user => user.Id)
            .HasColumnName(UserNaming.Columns.Id)
            .ValueGeneratedNever();

        userEntity
            .Property(user => user.Username)
            .HasColumnName(UserNaming.Columns.Username)
            .HasMaxLength(30)
            .IsRequired();

        userEntity
            .Property(user => user.Password)
            .HasColumnName(UserNaming.Columns.Password)
            .HasMaxLength(60)
            .IsRequired();

        userEntity
            .Property(user => user.DisplayName)
            .HasColumnName(UserNaming.Columns.DisplayName)
            .HasMaxLength(45)
            .IsRequired();

        userEntity
            .Navigation(user => user.RefreshTokens)
            .HasField("refreshTokens");
    }
}