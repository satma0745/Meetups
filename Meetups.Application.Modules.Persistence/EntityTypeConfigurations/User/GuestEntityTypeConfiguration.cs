namespace Meetups.Application.Modules.Persistence.EntityTypeConfigurations.User;

using Meetups.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class GuestEntityTypeConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> guestEntity) =>
        guestEntity
            .Navigation(guest => guest.MeetupsSignedUpTo)
            .HasField("meetupsSignedUpTo");
}