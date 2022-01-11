namespace Meetups.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddRefreshTokens : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.CreateTable(
            name: "RefreshTokens",
            columns: table => new
            {
                TokenId = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_RefreshTokens", x => x.TokenId));

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable("RefreshTokens");
}
