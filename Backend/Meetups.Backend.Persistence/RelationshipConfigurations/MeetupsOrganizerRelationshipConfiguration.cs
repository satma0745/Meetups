namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsOrganizerRelationshipConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity) =>
        meetupEntity
            .HasOne(meetup => meetup.Organizer)
            .WithMany(organizer => organizer.OrganizedMeetups)
            .HasForeignKey(MeetupNaming.Columns.OrganizerId)
            .HasConstraintName(MeetupNaming.ForeignKeys.OrganizerId)
            .IsRequired();
}