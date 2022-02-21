namespace Meetups.Backend.Persistence.EntityTypeConfigurations;

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
            .Property(meetup => meetup.Place)
            .HasColumnName("place")
            .HasMaxLength(75)
            .IsRequired();

        meetupEntity
            .Property(meetup => meetup.StartTime)
            .HasColumnName("start_time");

        meetupEntity
            .Property(meetup => meetup.Duration)
            .HasConversion(
                duration => duration.Hours * 60 + duration.Minutes,
                durationInMinutes => new MeetupDuration(durationInMinutes / 60, durationInMinutes % 60))
            .HasColumnName("duration")
            .IsRequired();
    }
}