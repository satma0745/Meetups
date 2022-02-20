namespace Meetups.Persistence.Migrations;

using System;
using Meetups.Backend.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20220116100522_FollowNamingConventions")]
partial class FollowNamingConventions
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
                .HasKey("Id")
                .HasName("pk_meetups");

            meetupEntity
                .HasIndex("Topic")
                .IsUnique()
                .HasDatabaseName("ux_meetups_topic");

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
        });

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsUsersJoinEntity =>
        {
            meetupsUsersJoinEntity
                .Property<Guid>("meetup_id")
                .HasColumnType("uuid");

            meetupsUsersJoinEntity
                .Property<Guid>("signed_up_user_id")
                .HasColumnType("uuid");

            meetupsUsersJoinEntity
                .HasKey("meetup_id", "signed_up_user_id")
                .HasName("pk_meetups_users_signup");

            meetupsUsersJoinEntity
                .HasIndex("signed_up_user_id")
                .HasDatabaseName("ix_meetups_users_signup_signed_up_user_id");

            meetupsUsersJoinEntity.ToTable("meetups_users_signup", (string) null);
        });

        modelBuilder.Entity("Meetups.Backend.Entities.Meetup.Meetup", meetupEntity =>
        {
            meetupEntity.OwnsOne(
                "Meetups.Backend.Entities.Meetup.MeetupDuration",
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

        modelBuilder.Entity("System.Collections.Generic.Dictionary<string, string>", meetupsUsersJoinEntity =>
        {
            meetupsUsersJoinEntity
                .HasOne("Meetups.Backend.Entities.Meetup.Meetup", null)
                .WithMany()
                .HasForeignKey("meetup_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_users_signup_meetups_signed_up_user_id");

            meetupsUsersJoinEntity
                .HasOne("Meetups.Backend.Entities.User.User", null)
                .WithMany()
                .HasForeignKey("signed_up_user_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_meetups_users_signup_users_signed_up_user_id");
        });
    }
}
