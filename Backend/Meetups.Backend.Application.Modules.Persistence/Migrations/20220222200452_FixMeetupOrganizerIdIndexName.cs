namespace Meetups.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class FixMeetupOrganizerIdIndexName : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.RenameIndex(
            name: "IX_meetups_organizer_id",
            table: "meetups",
            newName: "ix_meetups_organizer_id");

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.RenameIndex(
            name: "ix_meetups_organizer_id",
            table: "meetups",
            newName: "IX_meetups_organizer_id");
}