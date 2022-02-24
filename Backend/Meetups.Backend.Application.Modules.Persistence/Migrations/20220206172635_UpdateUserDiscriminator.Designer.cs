﻿namespace Meetups.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Meetups.Backend.Application.Modules.Persistence.Context;

[DbContext(typeof(ApplicationContext))]
[Migration("20220206172635_UpdateUserDiscriminator")]
partial class UpdateUserDiscriminator
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Meetups.Backend.Entities.Meetup.Meetup", meetupEntity =>
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
                .Property<Guid>("organizer_id")
                .HasColumnType("uuid");

            meetupEntity
                .HasKey("Id")
                .HasName("pk_meetups");

            meetupEntity
                .HasIndex("Topic")
                .IsUnique()
                .HasDatabaseName("ux_meetups_topic");

            meetupEntity.HasIndex("organizer_id");

            meetupEntity.ToTable("meetups", (string) null);
        });

        modelBuilder.Entity("Meetups.Backend.Entities.User.RefreshToken", refreshTokenEntity =>
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

        modelBuilder.Entity("Meetups.Backend.Entities.User.User", userEntity =>
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
                .Property<string>("Role")
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("role");

            userEntity
                .Property<string>("Username")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)")
                .HasColumnName("username");

            userEntity
                .HasKey("Id")
                .HasName("pk_users");

            userEntity
                .HasIndex("Username")
                .IsUnique()
                .HasDatabaseName("ux_users_username");

            userEntity.ToTable("users", (string) null);

            userEntity
                .HasDiscriminator<string>("Role")
                .HasValue("Guest");
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

        modelBuilder.Entity("Meetups.Backend.Entities.User.Guest", guestEntity =>
        {
            guestEntity.HasBaseType("Meetups.Backend.Entities.User.User");

            guestEntity
                .HasDiscriminator()
                .HasValue("Guest");
        });

        modelBuilder.Entity("Meetups.Backend.Entities.User.Organizer", organizerEntity =>
        {
            organizerEntity.HasBaseType("Meetups.Backend.Entities.User.User");

            organizerEntity
                .HasDiscriminator()
                .HasValue("Organizer");
        });

        modelBuilder.Entity("Meetups.Backend.Entities.Meetup.Meetup", meetupEntity =>
        {
            meetupEntity
                .HasOne("Meetups.Backend.Entities.User.Organizer", "Organizer")
                .WithMany("OrganizedMeetups")
                .HasForeignKey("organizer_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_organizers_organizer_id");

            meetupEntity.OwnsOne(
                "Meetups.Backend.Entities.Meetup.MeetupDuration",
                "Duration",
                meetupDurationOwnedEntity =>
                {
                    meetupDurationOwnedEntity
                        .Property<Guid>("MeetupId")
                        .HasColumnType("uuid");

                    meetupDurationOwnedEntity
                        .Property<int>("Hours")
                        .HasColumnType("integer")
                        .HasColumnName("duration_hours");

                    meetupDurationOwnedEntity
                        .Property<int>("Minutes")
                        .HasColumnType("integer")
                        .HasColumnName("duration_minutes");

                    meetupDurationOwnedEntity.HasKey("MeetupId");

                    meetupDurationOwnedEntity.ToTable("meetups");

                    meetupDurationOwnedEntity
                        .WithOwner()
                        .HasForeignKey("MeetupId");
                });

            meetupEntity
                .Navigation("Duration")
                .IsRequired();

            meetupEntity.Navigation("Organizer");
        });

        modelBuilder.Entity("Meetups.Backend.Entities.User.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .HasOne("Meetups.Backend.Entities.User.User", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_users_refresh_tokens_user_id");
        });

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsGuestsJoinEntity =>
        {
            meetupsGuestsJoinEntity
                .HasOne("Meetups.Backend.Entities.Meetup.Meetup", null)
                .WithMany()
                .HasForeignKey("meetup_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_meetups_meetup_id");

            meetupsGuestsJoinEntity
                .HasOne("Meetups.Backend.Entities.User.Guest", null)
                .WithMany()
                .HasForeignKey("signed_up_guest_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_guests_signed_up_guest_id");
        });

        modelBuilder.Entity("Meetups.Backend.Entities.User.Organizer", organizerEntity =>
        {
            organizerEntity.Navigation("OrganizedMeetups");
        });
    }
}