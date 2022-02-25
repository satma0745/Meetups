namespace Meetups.Application.Modules.Persistence.Migrations;

using System;
using Meetups.Application.Modules.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

[DbContext(typeof(ApplicationContext))]
internal class ApplicationContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.Meetup.City", cityEntity =>
        {
            cityEntity
                .Property<Guid>("Id")
                .HasColumnType("uuid")
                .HasColumnName("id");

            cityEntity
                .Property<string>("Name")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)")
                .HasColumnName("name");

            cityEntity
                .HasKey("Id")
                .HasName("pk_cities");

            cityEntity
                .HasIndex("Name")
                .IsUnique()
                .HasDatabaseName("ux_cities_name");

            cityEntity.ToTable("cities", (string) null);
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.Meetup.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
                .HasColumnType("uuid")
                .HasColumnName("id");

            meetupEntity
                .Property<int>("Duration")
                .HasColumnType("integer")
                .HasColumnName("duration");

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

            meetupEntity
                .HasIndex("organizer_id")
                .HasDatabaseName("ix_meetups_organizer_id");

            meetupEntity.ToTable("meetups", (string) null);
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .Property<Guid>("TokenId")
                .HasColumnType("uuid")
                .HasColumnName("token_id");

            refreshTokenEntity
                .Property<Guid>("BearerId")
                .HasColumnType("uuid")
                .HasColumnName("user_id");

            refreshTokenEntity
                .HasKey("TokenId")
                .HasName("pk_refresh_tokens");

            refreshTokenEntity
                .HasIndex("BearerId")
                .HasDatabaseName("ix_refresh_tokens_user_id");

            refreshTokenEntity.ToTable("refresh_tokens", (string) null);
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.User", userEntity =>
        {
            userEntity
                .Property<Guid>("Id")
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
                .Property<string>("role")
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
                .HasDiscriminator<string>("role")
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

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.Guest", guestEntity =>
        {
            guestEntity.HasBaseType("Meetups.Backend.Domain.Entities.User.User");

            guestEntity
                .HasDiscriminator()
                .HasValue("Guest");
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.Organizer", organizerEntity =>
        {
            organizerEntity.HasBaseType("Meetups.Backend.Domain.Entities.User.User");

            organizerEntity
                .HasDiscriminator()
                .HasValue("Organizer");
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.Meetup.Meetup", meetupEntity =>
        {
            meetupEntity
                .HasOne("Meetups.Backend.Domain.Entities.User.Organizer", "Organizer")
                .WithMany("OrganizedMeetups")
                .HasForeignKey("organizer_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_organizers_organizer_id");

            meetupEntity.OwnsOne("Meetups.Backend.Domain.Entities.Meetup.MeetupPlace", "Place",
                meetupPlaceOwnedEntity =>
                {
                    meetupPlaceOwnedEntity
                        .Property<Guid>("MeetupId")
                        .HasColumnType("uuid");

                    meetupPlaceOwnedEntity
                        .Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("place_address");

                    meetupPlaceOwnedEntity
                        .Property<Guid>("place_city_id")
                        .HasColumnType("uuid")
                        .HasColumnName("place_city_id");

                    meetupPlaceOwnedEntity.HasKey("MeetupId");

                    meetupPlaceOwnedEntity
                        .HasIndex("place_city_id")
                        .HasDatabaseName("ix_meetups_place_city_id");

                    meetupPlaceOwnedEntity.ToTable("meetups");

                    meetupPlaceOwnedEntity
                        .WithOwner()
                        .HasForeignKey("MeetupId");

                    meetupPlaceOwnedEntity.HasOne("Meetups.Backend.Domain.Entities.Meetup.City", "City")
                        .WithMany()
                        .HasForeignKey("place_city_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_meetups_cities_place_city_id");

                    meetupPlaceOwnedEntity.Navigation("City");
                });

            meetupEntity.Navigation("Organizer");

            meetupEntity
                .Navigation("Place")
                .IsRequired();
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .HasOne("Meetups.Backend.Domain.Entities.User.User", null)
                .WithMany("RefreshTokens")
                .HasForeignKey("BearerId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_users_refresh_tokens_user_id");
        });

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsGuestsJoinEntity =>
        {
            meetupsGuestsJoinEntity
                .HasOne("Meetups.Backend.Domain.Entities.Meetup.Meetup", null)
                .WithMany()
                .HasForeignKey("meetup_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_meetups_meetup_id");

            meetupsGuestsJoinEntity
                .HasOne("Meetups.Backend.Domain.Entities.User.Guest", null)
                .WithMany()
                .HasForeignKey("signed_up_guest_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_guests_signup_guests_signed_up_guest_id");
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.User", userEntity =>
        {
            userEntity.Navigation("RefreshTokens");
        });

        modelBuilder.Entity("Meetups.Backend.Domain.Entities.User.Organizer", organizerEntity =>
        {
            organizerEntity.Navigation("OrganizedMeetups");
        });
    }
}
