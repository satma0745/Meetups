namespace Meetups.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddCities : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "place",
            table: "meetups",
            newName: "place_address");

        migrationBuilder.AddColumn<Guid>(
            name: "place_city_id",
            table: "meetups",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "cities",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_cities", x => x.id));

        // Insert default city and assign all existing meetups to it
        var cityId = Guid.NewGuid();
        migrationBuilder.Sql($@"
            INSERT INTO cities(id, name)
            VALUES ('{cityId}', 'Oslo');
            UPDATE meetups
            SET place_city_id = '{cityId}';");

        migrationBuilder.CreateIndex(
            name: "ix_meetups_place_city_id",
            table: "meetups",
            column: "place_city_id");

        migrationBuilder.CreateIndex(
            name: "ux_cities_name",
            table: "cities",
            column: "name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_cities_place_city_id",
            table: "meetups",
            column: "place_city_id",
            principalTable: "cities",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_meetups_cities_place_city_id",
            table: "meetups");

        migrationBuilder.DropTable(
            name: "cities");

        migrationBuilder.DropIndex(
            name: "ix_meetups_place_city_id",
            table: "meetups");

        migrationBuilder.DropColumn(
            name: "place_city_id",
            table: "meetups");

        migrationBuilder.RenameColumn(
            name: "place_address",
            table: "meetups",
            newName: "place");
    }
}
