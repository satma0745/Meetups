namespace Meetups.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class FollowNamingConventions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_MeetupUser",
            table: "MeetupUser");

        migrationBuilder.DropForeignKey(
            name: "FK_MeetupUser_Meetups_MeetupsSignedUpToId",
            table: "MeetupUser");

        migrationBuilder.DropForeignKey(
            name: "FK_MeetupUser_Users_SignedUpUsersId",
            table: "MeetupUser");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Users",
            table: "Users");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Meetups",
            table: "Meetups");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RefreshTokens",
            table: "RefreshTokens");
        
        migrationBuilder.RenameTable(
            name: "MeetupUser",
            newName: "meetups_users_signup");

        migrationBuilder.RenameTable(
            name: "Users",
            newName: "users");

        migrationBuilder.RenameTable(
            name: "Meetups",
            newName: "meetups");

        migrationBuilder.RenameTable(
            name: "RefreshTokens",
            newName: "refresh_tokens");
        
        migrationBuilder.RenameColumn(
            name: "MeetupsSignedUpToId",
            table: "meetups_users_signup",
            newName: "meetup_id");

        migrationBuilder.RenameColumn(
            name: "SignedUpUsersId",
            table: "meetups_users_signup",
            newName: "signed_up_user_id");
        
        migrationBuilder.RenameIndex(
            name: "IX_MeetupUser_SignedUpUsersId",
            table: "meetups_users_signup",
            newName: "ix_meetups_users_signup_signed_up_user_id");

        migrationBuilder.RenameColumn(
            name: "Username",
            table: "users",
            newName: "username");

        migrationBuilder.RenameColumn(
            name: "Password",
            table: "users",
            newName: "password");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "users",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "DisplayName",
            table: "users",
            newName: "display_name");

        migrationBuilder.RenameIndex(
            name: "IX_Users_Username",
            table: "users",
            newName: "ux_users_username");

        migrationBuilder.RenameColumn(
            name: "Topic",
            table: "meetups",
            newName: "topic");

        migrationBuilder.RenameColumn(
            name: "Place",
            table: "meetups",
            newName: "place");

        migrationBuilder.RenameColumn(
            name: "Duration_Minutes",
            table: "meetups",
            newName: "duration_minutes");

        migrationBuilder.RenameColumn(
            name: "Duration_Hours",
            table: "meetups",
            newName: "duration_hours");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "meetups",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "StartTime",
            table: "meetups",
            newName: "start_time");

        migrationBuilder.RenameIndex(
            name: "IX_Meetups_Topic",
            table: "meetups",
            newName: "ux_meetups_topic");

        migrationBuilder.RenameColumn(
            name: "UserId",
            table: "refresh_tokens",
            newName: "user_id");

        migrationBuilder.RenameColumn(
            name: "TokenId",
            table: "refresh_tokens",
            newName: "token_id");
        
        migrationBuilder.AddPrimaryKey(
            name: "pk_meetups_users_signup",
            table: "meetups_users_signup",
            columns: new[] {"meetup_id", "signed_up_user_id"});

        migrationBuilder.AddPrimaryKey(
            name: "pk_users",
            table: "users",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "pk_meetups",
            table: "meetups",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "pk_refresh_tokens",
            table: "refresh_tokens",
            column: "token_id");

        migrationBuilder.CreateIndex(
            name: "ix_refresh_tokens_user_id",
            table: "refresh_tokens",
            column: "user_id");

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_users_signup_users_signed_up_user_id",
            table: "meetups_users_signup",
            column: "signed_up_user_id",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_meetups_users_signup_meetups_signed_up_user_id",
            table: "meetups_users_signup",
            column: "meetup_id",
            principalTable: "meetups",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_users_refresh_tokens_user_id",
            table: "refresh_tokens",
            column: "user_id",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "pk_meetups_users_signup",
            table: "meetups_users_signup");
        
        migrationBuilder.DropForeignKey(
            name: "fk_meetups_users_signup_users_signed_up_user_id",
            table: "meetups_users_signup");

        migrationBuilder.DropForeignKey(
            name: "fk_meetups_users_signup_meetups_signed_up_user_id",
            table: "meetups_users_signup");
        
        migrationBuilder.DropPrimaryKey(
            name: "pk_refresh_tokens",
            table: "refresh_tokens");

        migrationBuilder.DropIndex(
            name: "ix_refresh_tokens_user_id",
            table: "refresh_tokens");

        migrationBuilder.DropForeignKey(
            name: "fk_users_refresh_tokens_user_id",
            table: "refresh_tokens");

        migrationBuilder.DropPrimaryKey(
            name: "pk_users",
            table: "users");

        migrationBuilder.DropPrimaryKey(
            name: "pk_meetups",
            table: "meetups");
        
        migrationBuilder.RenameTable(
            name: "meetups_users_signup",
            newName: "MeetupUser");

        migrationBuilder.RenameTable(
            name: "users",
            newName: "Users");

        migrationBuilder.RenameTable(
            name: "meetups",
            newName: "Meetups");

        migrationBuilder.RenameTable(
            name: "refresh_tokens",
            newName: "RefreshTokens");
        
        migrationBuilder.RenameColumn(
            name: "meetup_id",
            table: "MeetupUser",
            newName: "MeetupsSignedUpToId");

        migrationBuilder.RenameColumn(
            name: "signed_up_user_id",
            table: "MeetupUser",
            newName: "SignedUpUsersId");
        
        migrationBuilder.RenameIndex(
            name: "ix_meetups_users_signup_signed_up_user_id",
            table: "MeetupUser",
            newName: "IX_MeetupUser_SignedUpUsersId");

        migrationBuilder.RenameColumn(
            name: "username",
            table: "Users",
            newName: "Username");

        migrationBuilder.RenameColumn(
            name: "password",
            table: "Users",
            newName: "Password");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Users",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "display_name",
            table: "Users",
            newName: "DisplayName");

        migrationBuilder.RenameIndex(
            name: "ux_users_username",
            table: "Users",
            newName: "IX_Users_Username");

        migrationBuilder.RenameColumn(
            name: "topic",
            table: "Meetups",
            newName: "Topic");

        migrationBuilder.RenameColumn(
            name: "place",
            table: "Meetups",
            newName: "Place");

        migrationBuilder.RenameColumn(
            name: "duration_minutes",
            table: "Meetups",
            newName: "Duration_Minutes");

        migrationBuilder.RenameColumn(
            name: "duration_hours",
            table: "Meetups",
            newName: "Duration_Hours");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Meetups",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "start_time",
            table: "Meetups",
            newName: "StartTime");

        migrationBuilder.RenameIndex(
            name: "ux_meetups_topic",
            table: "Meetups",
            newName: "IX_Meetups_Topic");

        migrationBuilder.RenameColumn(
            name: "user_id",
            table: "RefreshTokens",
            newName: "UserId");

        migrationBuilder.RenameColumn(
            name: "token_id",
            table: "RefreshTokens",
            newName: "TokenId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Users",
            table: "Users",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Meetups",
            table: "Meetups",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RefreshTokens",
            table: "RefreshTokens",
            column: "TokenId");
        
        migrationBuilder.AddPrimaryKey(
            name: "PK_MeetupUser",
            table: "MeetupUser",
            columns: new [] { "MeetupsSignedUpToId", "SignedUpUsersId" });

        migrationBuilder.AddForeignKey(
            name: "FK_MeetupUser_Meetups_MeetupsSignedUpToId",
            table: "MeetupUser",
            column: "MeetupsSignedUpToId",
            principalTable: "Meetups",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MeetupUser_Users_SignedUpUsersId",
            table: "MeetupUser",
            column: "SignedUpUsersId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
