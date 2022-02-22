namespace Meetups.Backend.Persistence.EntityTypeConfigurations.Meetup;

using System;
using Meetups.Backend.Entities.Meetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity)
    {
        meetupEntity.ToTable("meetups");

        meetupEntity
            .HasKey(meetup => meetup.Id)
            .HasName("pk_meetups");
        
        meetupEntity
            .HasIndex(meetup => meetup.Topic)
            .IsUnique()
            .HasDatabaseName("ux_meetups_topic");
        
        meetupEntity
            .Property(meetup => meetup.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        meetupEntity
            .Property(meetup => meetup.Topic)
            .HasColumnName("topic")
            .HasMaxLength(100)
            .IsRequired();
        meetupEntity
            .Property(meetup => meetup.StartTime)
            .HasColumnName("start_time");

        meetupEntity
            .Property(meetup => meetup.Duration)
            .HasConversion(
                duration => duration.TotalMinutes,
                totalMinutes => MeetupDuration.FromMinutes(totalMinutes))
            .HasColumnName("duration")
            .IsRequired();

        meetupEntity
            .OwnsOne(
                meetup => meetup.Place,
                placeOwnedEntity =>
                {
                    placeOwnedEntity.Property<Guid>("city_id");
                    
                    placeOwnedEntity
                        .HasIndex("city_id")
                        .HasDatabaseName("ix_meetups_place_city_id");
                    
                    placeOwnedEntity
                        .Property("city_id")
                        .HasColumnName("place_city_id")
                        .IsRequired();

                    placeOwnedEntity
                        .Property(place => place.Address)
                        .HasColumnName("place_address")
                        .HasMaxLength(75)
                        .IsRequired();

                    placeOwnedEntity
                        .HasOne(place => place.City)
                        .WithMany()
                        .HasForeignKey("city_id")
                        .HasConstraintName("fk_meetups_cities_place_city_id")
                        .IsRequired();
                })
            .Navigation(meetup => meetup.Place)
            .IsRequired();
    }
}