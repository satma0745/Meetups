namespace Meetups.Persistence.RelationshipConfigurations;

using System.Collections.Generic;
using Meetups.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsGuestsSignUpRelationshipConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity) =>
        meetupEntity
            .HasMany(x => x.SignedUpGuests)
            .WithMany(x => x.MeetupsSignedUpTo)
            .UsingEntity<Dictionary<string, string>>(
                guestSide => guestSide
                    .HasOne<Guest>()
                    .WithMany()
                    .HasForeignKey("signed_up_guest_id")
                    .HasConstraintName("fk_meetups_guests_signup_guests_signed_up_guest_id"),
                meetupsSide => meetupsSide
                    .HasOne<Meetup>()
                    .WithMany()
                    .HasForeignKey("meetup_id")
                    .HasConstraintName("fk_meetups_guests_signup_meetups_meetup_id"),
                joinEntity =>
                {
                    joinEntity.ToTable("meetups_guests_signup");

                    joinEntity
                        .HasKey("meetup_id", "signed_up_guest_id")
                        .HasName("pk_meetups_guests_signup");

                    joinEntity
                        .HasIndex("signed_up_guest_id")
                        .HasDatabaseName("ix_meetups_guests_signup_signed_up_guest_id");
                });
}