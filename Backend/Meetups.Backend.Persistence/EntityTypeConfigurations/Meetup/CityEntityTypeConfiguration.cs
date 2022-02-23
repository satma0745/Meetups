namespace Meetups.Backend.Persistence.EntityTypeConfigurations.Meetup;

using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> cityEntity)
    {
        cityEntity.ToTable(CityNaming.Table);

        cityEntity
            .HasKey(city => city.Id)
            .HasName(CityNaming.Indices.PrimaryKey);

        cityEntity
            .HasIndex(city => city.Name)
            .IsUnique()
            .HasDatabaseName(CityNaming.Indices.UniqueName);
        
        cityEntity
            .Property(city => city.Id)
            .HasColumnName(CityNaming.Columns.Id)
            .ValueGeneratedNever();

        cityEntity
            .Property(city => city.Name)
            .HasColumnName(CityNaming.Columns.Name)
            .HasMaxLength(30)
            .IsRequired();
    }
}