namespace Meetups.Backend.Persistence.EntityTypeConfigurations.Meetup;

using System;
using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity)
    {
        meetupEntity.ToTable(MeetupNaming.Table);

        meetupEntity.Property<Guid>(MeetupNaming.Columns.OrganizerId);

        meetupEntity
            .HasKey(meetup => meetup.Id)
            .HasName(MeetupNaming.Indices.PrimaryKey);
        
        meetupEntity
            .HasIndex(meetup => meetup.Topic)
            .IsUnique()
            .HasDatabaseName(MeetupNaming.Indices.UniqueTopic);

        meetupEntity
            .HasIndex(MeetupNaming.Columns.OrganizerId)
            .HasDatabaseName(MeetupNaming.Indices.OrganizerId);
        
        meetupEntity
            .Property(meetup => meetup.Id)
            .HasColumnName(MeetupNaming.Columns.Id)
            .ValueGeneratedNever();

        meetupEntity
            .Property(meetup => meetup.Topic)
            .HasColumnName(MeetupNaming.Columns.Topic)
            .HasMaxLength(100)
            .IsRequired();
        
        meetupEntity
            .Property(meetup => meetup.StartTime)
            .HasColumnName(MeetupNaming.Columns.StartTime);

        meetupEntity
            .Property(meetup => meetup.Duration)
            .HasConversion(
                duration => duration.TotalMinutes,
                totalMinutes => MeetupDuration.FromMinutes(totalMinutes))
            .HasColumnName(MeetupNaming.Columns.Duration)
            .IsRequired();

        meetupEntity
            .OwnsOne(
                meetup => meetup.Place,
                placeOwnedEntity =>
                {
                    placeOwnedEntity.Property<Guid>("city_id");
                    
                    placeOwnedEntity
                        .HasIndex("city_id")
                        .HasDatabaseName(MeetupNaming.Indices.CityId);
                    
                    placeOwnedEntity
                        .Property("city_id")
                        .HasColumnName(MeetupNaming.Columns.CityId)
                        .IsRequired();

                    placeOwnedEntity
                        .Property(place => place.Address)
                        .HasColumnName(MeetupNaming.Columns.Address)
                        .HasMaxLength(75)
                        .IsRequired();

                    placeOwnedEntity
                        .HasOne(place => place.City)
                        .WithMany()
                        .HasForeignKey("city_id")
                        .HasConstraintName(MeetupNaming.ForeignKeys.CityId)
                        .IsRequired();
                })
            .Navigation(meetup => meetup.Place)
            .IsRequired();
    }
}