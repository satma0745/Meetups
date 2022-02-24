namespace Meetups.Backend.Application.Modules.Persistence.EntityTypeConfigurations.User;

using Meetups.Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class OrganizerEntityTypeConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> organizerEntity) =>
        organizerEntity
            .Navigation(organizer => organizer.OrganizedMeetups)
            .HasField("organizedMeetups");
}