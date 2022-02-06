namespace Meetups.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddOrganizers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"DELETE FROM meetups;");

        migrationBuilder.AddColumn<Guid>(
            name: "organizer_id",
            table: "meetups",
            type: "uuid",
            nullable: false);

        migrationBuilder.CreateIndex(
            name: "IX_meetups_organizer_id",
            table: "meetups",
            column: "organizer_id");

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_organizers_organizer_id",
            table: "meetups",
            column: "organizer_id",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_meetups_organizers_organizer_id",
            table: "meetups");

        migrationBuilder.DropIndex(
            name: "IX_meetups_organizer_id",
            table: "meetups");

        migrationBuilder.DropColumn(
            name: "organizer_id",
            table: "meetups");
    }
}
