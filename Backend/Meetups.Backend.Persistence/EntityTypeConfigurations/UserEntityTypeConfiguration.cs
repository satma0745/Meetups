namespace Meetups.Backend.Persistence.EntityTypeConfigurations;

using Meetups.Backend.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userEntity)
    {
        userEntity.ToTable("users");

        userEntity
            .HasDiscriminator<string>("role")
            .HasValue<Guest>(nameof(Guest))
            .HasValue<Organizer>(nameof(Organizer))
            .HasValue(nameof(Guest));

        userEntity
            .HasKey(user => user.Id)
            .HasName("pk_users");

        userEntity
            .HasIndex(user => user.Username)
            .IsUnique()
            .HasDatabaseName("ux_users_username");

        userEntity
            .Property(user => user.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        userEntity
            .Property(user => user.Username)
            .HasColumnName("username")
            .HasMaxLength(30)
            .IsRequired();

        userEntity
            .Property(user => user.Password)
            .HasColumnName("password")
            .HasMaxLength(60)
            .IsRequired();

        userEntity
            .Property(user => user.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(45)
            .IsRequired();

        userEntity
            .Navigation(user => user.RefreshTokens)
            .HasField("refreshTokens");
    }
}