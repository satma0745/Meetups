﻿namespace Meetups.Backend.Persistence.EntityTypeConfigurations.User;

using Meetups.Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class GuestEntityTypeConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> guestEntity) =>
        guestEntity
            .Navigation(guest => guest.MeetupsSignedUpTo)
            .HasField("meetupsSignedUpTo");
}