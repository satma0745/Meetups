namespace Meetups.Persistence.RelationshipConfigurations;

using System.Collections.Generic;
using Meetups.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MeetupsUsersSignUpRelationshipConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity) =>
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