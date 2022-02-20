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
            .HasKey(x => x.Id)
            .HasName("pk_users");

        userEntity
            .HasIndex(x => x.Username)
            .IsUnique()
            .HasDatabaseName("ux_users_username");

        userEntity
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        userEntity
            .Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(30)
            .IsRequired();

        userEntity
            .Property(x => x.Password)
            .HasColumnName("password")
            .HasMaxLength(60)
            .IsRequired();

        userEntity
            .Property(x => x.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(45)
            .IsRequired();
    }
}