using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "UserRefreshTokens",
                newName: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "UserRefreshTokens",
                newName: "Token");
        }
    }
}
