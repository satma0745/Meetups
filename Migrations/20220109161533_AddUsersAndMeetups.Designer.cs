﻿namespace Meetups.Migrations;

using System;
using Meetups.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20220109161533_AddUsersAndMeetups")]
partial class AddUsersAndMeetups
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Meetups.Entities.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            meetupEntity
                .Property<TimeSpan>("Duration")
                .HasColumnType("interval");

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

        modelBuilder.Entity("Meetups.Entities.User", userEntity =>
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
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

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
    }
}
