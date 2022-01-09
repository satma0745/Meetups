namespace Meetups.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddUsersAndMeetups : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Meetups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Topic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Place = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Meetups", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                Password = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                DisplayName = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Meetups_Topic",
            table: "Meetups",
            column: "Topic",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            table: "Users",
            column: "Username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Meetups");
        migrationBuilder.DropTable("Users");
    }
}
