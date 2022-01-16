namespace Meetups.Persistence.EntityTypeConfigurations;

using System.Collections.Generic;
using Meetups.Persistence.Entities;
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
            .HasColumnName("id");

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
            .OwnsOne(
                x => x.Duration,
                durationOwnedEntity =>
                {
                    durationOwnedEntity
                        .Property(x => x.Hours)
                        .HasColumnName("duration_hours");

                    durationOwnedEntity
                        .Property(x => x.Minutes)
                        .HasColumnName("duration_minutes");
                })
            .Navigation(x => x.Duration)
            .IsRequired();

        meetupEntity
            .HasMany(x => x.SignedUpUsers)
            .WithMany(x => x.MeetupsSignedUpTo)
            .UsingEntity<Dictionary<string, string>>(
                usersSide => usersSide
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("signed_up_user_id")
                    .HasConstraintName("fk_meetups_users_signup_users_signed_up_user_id"),
                meetupsSide => meetupsSide
                    .HasOne<Meetup>()
                    .WithMany()
                    .HasForeignKey("meetup_id")
                    .HasConstraintName("fk_meetups_users_signup_meetups_signed_up_user_id"),
                joinEntity =>
                {
                    joinEntity.ToTable("meetups_users_signup");

                    joinEntity
                        .HasKey("meetup_id", "signed_up_user_id")
                        .HasName("pk_meetups_users_signup");

                    joinEntity
                        .HasIndex("signed_up_user_id")
                        .HasDatabaseName("ix_meetups_users_signup_signed_up_user_id");
                });
    }
}