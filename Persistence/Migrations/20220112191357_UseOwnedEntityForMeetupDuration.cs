namespace Meetups.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class UseOwnedEntityForMeetupDuration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Duration",
            table: "Meetups");

        migrationBuilder.AddColumn<int>(
            name: "Duration_Hours",
            table: "Meetups",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Duration_Minutes",
            table: "Meetups",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Duration_Hours",
            table: "Meetups");

        migrationBuilder.DropColumn(
            name: "Duration_Minutes",
            table: "Meetups");

        migrationBuilder.AddColumn<TimeSpan>(
            name: "Duration",
            table: "Meetups",
            type: "interval",
            nullable: false,
            defaultValue: new TimeSpan(0, 0, 0, 0, 0));
    }
}
