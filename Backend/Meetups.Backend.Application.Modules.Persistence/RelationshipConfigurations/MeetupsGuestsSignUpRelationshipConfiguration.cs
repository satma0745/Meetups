namespace Meetups.Backend.Application.Modules.Persistence.RelationshipConfigurations;

using System.Collections.Generic;
using Meetups.Backend.Application.Modules.Persistence.Naming;
using Meetups.Backend.Domain.Entities.Meetup;
using Meetups.Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsGuestsSignUpRelationshipConfiguration : IRelationshipConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity) =>
        meetupEntity
            .HasMany(x => x.SignedUpGuests)
            .WithMany(x => x.MeetupsSignedUpTo)
            .UsingEntity<Dictionary<string, string>>(
                guestSide => guestSide
                    .HasOne<Guest>()
                    .WithMany()
                    .HasForeignKey(MeetupsGuestsSignupSkipNaming.Columns.GuestId)
                    .HasConstraintName(MeetupsGuestsSignupSkipNaming.ForeignKeys.GuestId),
                meetupsSide => meetupsSide
                    .HasOne<Meetup>()
                    .WithMany()
                    .HasForeignKey(MeetupsGuestsSignupSkipNaming.Columns.MeetupId)
                    .HasConstraintName(MeetupsGuestsSignupSkipNaming.ForeignKeys.MeetupId),
                joinEntity =>
                {
                    joinEntity.ToTable(MeetupsGuestsSignupSkipNaming.Table);

                    joinEntity
                        .HasKey(
                            MeetupsGuestsSignupSkipNaming.Columns.MeetupId,
                            MeetupsGuestsSignupSkipNaming.Columns.GuestId)
                        .HasName(MeetupsGuestsSignupSkipNaming.Indices.PrimaryKey);

                    joinEntity
                        .HasIndex(MeetupsGuestsSignupSkipNaming.Columns.GuestId)
                        .HasDatabaseName(MeetupsGuestsSignupSkipNaming.Indices.GuestId);
                });
}