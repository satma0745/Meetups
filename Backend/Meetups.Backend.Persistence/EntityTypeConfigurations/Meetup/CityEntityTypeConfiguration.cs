namespace Meetups.Backend.Persistence.EntityTypeConfigurations.Meetup;

using Meetups.Backend.Entities.Meetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> cityEntity)
    {
        cityEntity.ToTable("cities");

        cityEntity
            .HasKey(city => city.Id)
            .HasName("pk_cities");

        cityEntity
            .HasIndex(city => city.Name)
            .IsUnique()
            .HasDatabaseName("ux_cities_name");
        
        cityEntity
            .Property(city => city.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        cityEntity
            .Property(city => city.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();
    }
}