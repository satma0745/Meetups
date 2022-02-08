namespace Meetups.Backend.Persistence.EntityTypeConfigurations;

using Meetups.Backend.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity)
    {
        meetupEntity.ToTable("meetups");

        meetupEntity
            .HasKey(x => x.Id)
            .HasName("pk_meetups");
        
        meetupEntity
            .HasIndex(x => x.Topic)
            .IsUnique()
            .HasDatabaseName("ux_meetups_topic");
        
        meetupEntity
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        meetupEntity
            .Property(x => x.Topic)
            .HasColumnName("topic")
            .HasMaxLength(100)
            .IsRequired();

        meetupEntity
            .Property(x => x.Place)
            .HasColumnName("place")
            .HasMaxLength(75)
            .IsRequired();

        meetupEntity
            .Property(x => x.StartTime)
            .HasColumnName("start_time");

        meetupEntity
            .Property(x => x.Duration)
            .HasConversion(
                duration => duration.Hours * 60 + duration.Minutes,
                durationInMinutes => new Meetup.MeetupDuration
                {
                    Hours = durationInMinutes / 60,
                    Minutes = durationInMinutes % 60
                },
                new ValueComparer<Meetup.MeetupDuration>(
                    (duration1, duration2) => duration1.Hours == duration2.Hours &&
                                              duration1.Minutes == duration2.Minutes,
                    duration => duration.Hours * 60 + duration.Minutes,
                    duration => new Meetup.MeetupDuration
                    {
                        Hours = duration.Hours,
                        Minutes = duration.Minutes
                    }))
            .HasColumnName("duration")
            .IsRequired();

        // .OwnsOne(
        //     x => x.Duration,
        //     durationOwnedEntity =>
        //     {
        //         durationOwnedEntity
        //             .Property(x => x.Hours)
        //             .HasColumnName("duration_hours");
        //
        //         durationOwnedEntity
        //             .Property(x => x.Minutes)
        //             .HasColumnName("duration_minutes");
        //     })
        // .Navigation(x => x.Duration)
        // .IsRequired();
    }
}