namespace Meetups.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class UpdateUserDiscriminator : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "account_type",
            table: "users",
            newName: "role");

        migrationBuilder.Sql(@"
            UPDATE users
            SET role = 'Guest'
            WHERE role = 'guest';
            UPDATE users
            SET role = 'Organizer'
            WHERE role = 'organizer';");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "role",
            table: "users",
            newName: "account_type");

        migrationBuilder.Sql(@"
            UPDATE users
            SET account_type = 'guest'
            WHERE account_type = 'Guest';
            UPDATE users
            SET account_type = 'organizer'
            WHERE account_type = 'Organizer';");
    }
}