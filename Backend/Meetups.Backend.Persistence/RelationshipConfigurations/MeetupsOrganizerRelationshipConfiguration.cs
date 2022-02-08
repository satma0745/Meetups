namespace Meetups.Backend.Persistence.RelationshipConfigurations;

using Meetups.Backend.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsOrganizerRelationshipConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity) =>
        meetupEntity
            .HasOne(meetup => meetup.Organizer)
            .WithMany(organizer => organizer.OrganizedMeetups)
            .HasForeignKey("organizer_id")
            .HasConstraintName("fk_meetups_organizers_organizer_id")
            .IsRequired();
}