namespace Meetups.Application.Modules.Persistence.EntityTypeConfigurations.Meetup;

using System;
using Meetups.Application.Modules.Persistence.Naming;
using Meetups.Domain.Entities.Meetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity)
    {
        meetupEntity.ToTable(MeetupNaming.Table);

        // Shadow property
        meetupEntity.Property<Guid>(MeetupNaming.Columns.OrganizerId);

        meetupEntity
            .HasKey(meetup => meetup.Id)
            .HasName(MeetupNaming.Indices.PrimaryKey);
        
        meetupEntity
            .HasIndex(meetup => meetup.Topic)
            .IsUnique()
            .HasDatabaseName(MeetupNaming.Indices.UniqueTopic);
        
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
                    // Shadow property
                    placeOwnedEntity
                        .Property<Guid>(MeetupNaming.Columns.CityId)
                        .HasColumnName(MeetupNaming.Columns.CityId);
                    
                    placeOwnedEntity
                        .HasIndex(MeetupNaming.Columns.CityId)
                        .HasDatabaseName(MeetupNaming.Indices.CityId);

                    placeOwnedEntity
                        .Property(place => place.Address)
                        .HasColumnName(MeetupNaming.Columns.Address)
                        .HasMaxLength(75)
                        .IsRequired();

                    placeOwnedEntity
                        .HasOne(place => place.City)
                        .WithMany()
                        .HasForeignKey(MeetupNaming.Columns.CityId)
                        .HasConstraintName(MeetupNaming.ForeignKeys.CityId)
                        .IsRequired();
                })
            .Navigation(meetup => meetup.Place)
            .IsRequired();
    }
}