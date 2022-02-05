using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetups.Persistence.Migrations
{
    public partial class AddGuests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Temporarily drop foreign keys
            
            migrationBuilder.DropForeignKey(
                name: "fk_meetups_users_signup_meetups_signed_up_user_id",
                table: "meetups_users_signup");

            migrationBuilder.DropForeignKey(
                name: "fk_meetups_users_signup_users_signed_up_user_id",
                table: "meetups_users_signup");
            
            #endregion

            #region Rename join table, it's column, index and primary key
            
            migrationBuilder.DropPrimaryKey(
                name: "pk_meetups_users_signup",
                table: "meetups_users_signup");

            migrationBuilder.RenameTable(
                name: "meetups_users_signup",
                newName: "meetups_guests_signup");

            migrationBuilder.RenameColumn(
                name: "signed_up_user_id",
                table: "meetups_guests_signup",
                newName: "signed_up_guest_id");

            migrationBuilder.RenameIndex(
                name: "ix_meetups_users_signup_signed_up_user_id",
                table: "meetups_guests_signup",
                newName: "ix_meetups_guests_signup_signed_up_guest_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_meetups_guests_signup",
                table: "meetups_guests_signup",
                columns: new[] { "meetup_id", "signed_up_guest_id" });
            
            #endregion
            
            #region Add discriminator column to users table
            
            migrationBuilder.AddColumn<string>(
                name: "account_type",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "guest");
            
            #endregion

            #region Recreate dropped foreign keys
            
            migrationBuilder.AddForeignKey(
                name: "fk_meetups_guests_signup_guests_signed_up_guest_id",
                table: "meetups_guests_signup",
                column: "signed_up_guest_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_meetups_guests_signup_meetups_meetup_id",
                table: "meetups_guests_signup",
                column: "meetup_id",
                principalTable: "meetups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
            
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            #region Temporarily drop foreign keys
            
            migrationBuilder.DropForeignKey(
                name: "fk_meetups_guests_signup_guests_signed_up_guest_id",
                table: "meetups_guests_signup");

            migrationBuilder.DropForeignKey(
                name: "fk_meetups_guests_signup_meetups_meetup_id",
                table: "meetups_guests_signup");
            
            #endregion

            #region Rename join table, it's column, index and primary key
            
            migrationBuilder.DropPrimaryKey(
                name: "pk_meetups_guests_signup",
                table: "meetups_guests_signup");

            migrationBuilder.RenameTable(
                name: "meetups_guests_signup",
                newName: "meetups_users_signup");

            migrationBuilder.RenameColumn(
                name: "signed_up_guest_id",
                table: "meetups_users_signup",
                newName: "signed_up_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_meetups_guests_signup_signed_up_guest_id",
                table: "meetups_users_signup",
                newName: "ix_meetups_users_signup_signed_up_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_meetups_users_signup",
                table: "meetups_users_signup",
                columns: new[] { "meetup_id", "signed_up_user_id" });
            
            #endregion
            
            #region Drop discriminator column from users table

            migrationBuilder.DropColumn(
                name: "account_type",
                table: "users");
            
            #endregion

            #region Recreate dropped foreign keys
            
            migrationBuilder.AddForeignKey(
                name: "fk_meetups_users_signup_meetups_signed_up_user_id",
                table: "meetups_users_signup",
                column: "meetup_id",
                principalTable: "meetups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_meetups_users_signup_users_signed_up_user_id",
                table: "meetups_users_signup",
                column: "signed_up_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
            
            #endregion
        }
    }
}
