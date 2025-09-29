using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRefreshToken_Users_UserId",
                table: "UsersRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRefreshToken",
                table: "UsersRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_UsersRefreshToken_UserId",
                table: "UsersRefreshToken");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "UsersRefreshToken");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "UsersRefreshToken");

            migrationBuilder.DropColumn(
                name: "JwtId",
                table: "UsersRefreshToken");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UsersRefreshToken");

            migrationBuilder.RenameTable(
                name: "UsersRefreshToken",
                newName: "UserRefreshToken");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "UserRefreshToken",
                newName: "ExpiresOn");

            migrationBuilder.RenameColumn(
                name: "AddedTime",
                table: "UserRefreshToken",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserRefreshToken",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedOn",
                table: "UserRefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken",
                columns: new[] { "UserId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "RevokedOn",
                table: "UserRefreshToken");

            migrationBuilder.RenameTable(
                name: "UserRefreshToken",
                newName: "UsersRefreshToken");

            migrationBuilder.RenameColumn(
                name: "ExpiresOn",
                table: "UsersRefreshToken",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "UsersRefreshToken",
                newName: "AddedTime");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UsersRefreshToken",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "UsersRefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "UsersRefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JwtId",
                table: "UsersRefreshToken",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UsersRefreshToken",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRefreshToken",
                table: "UsersRefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRefreshToken_UserId",
                table: "UsersRefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRefreshToken_Users_UserId",
                table: "UsersRefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
