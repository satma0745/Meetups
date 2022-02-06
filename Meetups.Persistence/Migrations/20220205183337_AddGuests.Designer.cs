namespace Meetups.Persistence.Migrations;

using System;
using Meetups.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20220205183337_AddGuests")]
partial class AddGuests
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Meetups.Persistence.Entities.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid")
                .HasColumnName("id");

            meetupEntity
                .Property<string>("Place")
                .IsRequired()
                .HasMaxLength(75)
                .HasColumnType("character varying(75)")
                .HasColumnName("place");

            meetupEntity
                .Property<DateTime>("StartTime")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("start_time");

            meetupEntity
                .Property<string>("Topic")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)")
                .HasColumnName("topic");

            meetupEntity
                .HasKey("Id")
                .HasName("pk_meetups");

            meetupEntity
                .HasIndex("Topic")
                .IsUnique()
                .HasDatabaseName("ux_meetups_topic");

            meetupEntity.ToTable("meetups", (string) null);
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .Property<Guid>("TokenId")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid")
                .HasColumnName("token_id");

            refreshTokenEntity
                .Property<Guid>("UserId")
                .HasColumnType("uuid")
                .HasColumnName("user_id");

            refreshTokenEntity
                .HasKey("TokenId")
                .HasName("pk_refresh_tokens");

            refreshTokenEntity
                .HasIndex("UserId")
                .HasDatabaseName("ix_refresh_tokens_user_id");

            refreshTokenEntity.ToTable("refresh_tokens", (string) null);
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.User", userEntity =>
        {
            userEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid")
                .HasColumnName("id");

            userEntity
                .Property<string>("DisplayName")
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnType("character varying(45)")
                .HasColumnName("display_name");

            userEntity
                .Property<string>("Password")
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnType("character varying(60)")
                .HasColumnName("password");

            userEntity
                .Property<string>("Username")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)")
                .HasColumnName("username");

            userEntity
                .Property<string>("account_type")
                .IsRequired()
                .HasColumnType("text");

            userEntity
                .HasKey("Id")
                .HasName("pk_users");

            userEntity
                .HasIndex("Username")
                .IsUnique()
                .HasDatabaseName("ux_users_username");

            userEntity.ToTable("users", (string) null);

            userEntity
                .HasDiscriminator<string>("account_type")
                .HasValue("guest");
        });

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsGuestsJoinEntity =>
        {
            meetupsGuestsJoinEntity
                .Property<Guid>("meetup_id")
                .HasColumnType("uuid");

            meetupsGuestsJoinEntity
                .Property<Guid>("signed_up_guest_id")
                .HasColumnType("uuid");

            meetupsGuestsJoinEntity
                .HasKey("meetup_id", "signed_up_guest_id")
                .HasName("pk_meetups_guests_signup");

            meetupsGuestsJoinEntity
                .HasIndex("signed_up_guest_id")
                .HasDatabaseName("ix_meetups_guests_signup_signed_up_guest_id");

            meetupsGuestsJoinEntity.ToTable("meetups_guests_signup", (string) null);
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.Guest", guestEntity =>
        {
            guestEntity.HasBaseType("Meetups.Persistence.Entities.User");

            guestEntity
                .HasDiscriminator()
                .HasValue("guest");
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.Meetup", meetupEntity =>
        {
            meetupEntity.OwnsOne(
                "Meetups.Persistence.Entities.Meetup+MeetupDuration",
                "Duration",
                durationOwnedEntity =>
                {
                    durationOwnedEntity
                        .Property<Guid>("MeetupId")
                        .HasColumnType("uuid");

                    durationOwnedEntity
                        .Property<int>("Hours")
                        .HasColumnType("integer")
                        .HasColumnName("duration_hours");

                    durationOwnedEntity
                        .Property<int>("Minutes")
                        .HasColumnType("integer")
                        .HasColumnName("duration_minutes");

                    durationOwnedEntity.HasKey("MeetupId");

                    durationOwnedEntity.ToTable("meetups");

                    durationOwnedEntity
                        .WithOwner()
                        .HasForeignKey("MeetupId");
                });

            meetupEntity
                .Navigation("Duration")
                .IsRequired();
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .HasOne("Meetups.Persistence.Entities.User", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_users_refresh_tokens_user_id");
        });

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsGuestsJoinEntity =>
        {
            meetupsGuestsJoinEntity
                .HasOne("Meetups.Persistence.Entities.Meetup", null)
                .WithMany()
                .HasForeignKey("meetup_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_meetups_meetup_id");

            meetupsGuestsJoinEntity
                .HasOne("Meetups.Persistence.Entities.Guest", null)
                .WithMany()
                .HasForeignKey("signed_up_guest_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_guests_signed_up_guest_id");
        });
    }
}
