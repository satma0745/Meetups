namespace Meetups.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class IncreasePasswordLength : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "Users",
            type: "character varying(60)",
            maxLength: 60,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(30)",
            oldMaxLength: 30);

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "Users",
            type: "character varying(30)",
            maxLength: 30,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(60)",
            oldMaxLength: 60);
}