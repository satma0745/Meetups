namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Domain.Entities.Meetup;
using Meetups.Backend.Domain.Entities.User;
using Meetups.Backend.Persistence.Naming;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsOrganizerRelationshipConfiguration : IRelationshipConfiguration<Meetup, Organizer>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity, EntityTypeBuilder<Organizer> organizerEntity)
    {
        meetupEntity
            .HasIndex(MeetupNaming.Columns.OrganizerId)
            .HasDatabaseName(MeetupNaming.Indices.OrganizerId);

        organizerEntity
            .HasMany(organizer => organizer.OrganizedMeetups)
            .WithOne(meetup => meetup.Organizer)
            .HasForeignKey(MeetupNaming.Columns.OrganizerId)
            .HasConstraintName(MeetupNaming.ForeignKeys.OrganizerId)
            .IsRequired();
    }
}