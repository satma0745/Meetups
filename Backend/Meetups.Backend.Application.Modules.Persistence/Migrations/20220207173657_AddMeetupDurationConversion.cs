namespace Meetups.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddMeetupDurationConversion : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "duration",
            table: "meetups",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql(@"
            UPDATE meetups
            SET duration = duration_hours * 60 + duration_minutes;");
        
        migrationBuilder.DropColumn(
            name: "duration_hours",
            table: "meetups");
        
        migrationBuilder.DropColumn(
            name: "duration_minutes",
            table: "meetups");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "duration_hours",
            table: "meetups",
            type: "integer",
            nullable: false,
            defaultValue: 0);
        
        migrationBuilder.AddColumn<int>(
            name: "duration_minutes",
            table: "meetups",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql(@"
            UPDATE meetups
            SET duration_hours = duration / 60,
                duration_minutes = MOD(duration, 60);");
        
        migrationBuilder.DropColumn(
            name: "duration",
            table: "meetups");
    }
}
