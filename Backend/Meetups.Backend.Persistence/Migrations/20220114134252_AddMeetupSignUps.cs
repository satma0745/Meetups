namespace Meetups.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddMeetupSignUps : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MeetupUser",
            columns: table => new
            {
                MeetupsSignedUpToId = table.Column<Guid>(type: "uuid", nullable: false),
                SignedUpUsersId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MeetupUser", x => new { x.MeetupsSignedUpToId, x.SignedUpUsersId });
                table.ForeignKey(
                    name: "FK_MeetupUser_Meetups_MeetupsSignedUpToId",
                    column: x => x.MeetupsSignedUpToId,
                    principalTable: "Meetups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MeetupUser_Users_SignedUpUsersId",
                    column: x => x.SignedUpUsersId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MeetupUser_SignedUpUsersId",
            table: "MeetupUser",
            column: "SignedUpUsersId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable("MeetupUser");
}
