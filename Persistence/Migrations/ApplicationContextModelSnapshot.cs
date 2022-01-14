﻿namespace Meetups.Persistence.Migrations;

using System;
using Meetups.Persistence.Context;
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

        modelBuilder.Entity("Meetups.Persistence.Entities.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            meetupEntity
                .Property<string>("Place")
                .IsRequired()
                .HasMaxLength(75)
                .HasColumnType("character varying(75)");

            meetupEntity
                .Property<DateTime>("StartTime")
                .HasColumnType("timestamp with time zone");

            meetupEntity
                .Property<string>("Topic")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            meetupEntity.HasKey("Id");

            meetupEntity
                .HasIndex("Topic")
                .IsUnique();

            meetupEntity.ToTable("Meetups");
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .Property<Guid>("TokenId")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            refreshTokenEntity
                .Property<Guid>("UserId")
                .HasColumnType("uuid");

            refreshTokenEntity.HasKey("TokenId");

            refreshTokenEntity.ToTable("RefreshTokens");
        });

        modelBuilder.Entity("Meetups.Persistence.Entities.User", userEntity =>
        {
            userEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            userEntity
                .Property<string>("DisplayName")
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnType("character varying(45)");

            userEntity
                .Property<string>("Password")
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnType("character varying(60)");

            userEntity
                .Property<string>("Username")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

            userEntity.HasKey("Id");

            userEntity
                .HasIndex("Username")
                .IsUnique();

            userEntity.ToTable("Users");
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
                        .HasColumnType("integer");

                    durationOwnedEntity
                        .Property<int>("Minutes")
                        .HasColumnType("integer");

                    durationOwnedEntity.HasKey("MeetupId");

                    durationOwnedEntity.ToTable("Meetups");

                    durationOwnedEntity
                        .WithOwner()
                        .HasForeignKey("MeetupId");
                });

            meetupEntity
                .Navigation("Duration")
                .IsRequired();
        });
    }
}
