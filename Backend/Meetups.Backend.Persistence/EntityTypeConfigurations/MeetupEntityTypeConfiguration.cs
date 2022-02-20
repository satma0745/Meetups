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
                durationInMinutes => new MeetupDuration(durationInMinutes / 60, durationInMinutes % 60))
            .HasColumnName("duration")
            .IsRequired();
    }
}