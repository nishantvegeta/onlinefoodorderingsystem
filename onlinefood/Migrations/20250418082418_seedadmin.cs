using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlinefood.Migrations
{
    /// <inheritdoc />
    public partial class seedadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$vYvdlAR/4cXJBFHs3LsOhuGt75LSUazrHsW7skY/Lc4rr/CsCDfPC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2XobGaY7WQkTwv06j6mCrOaK5nBy65A24veCbWH.3Omj7JGkTTnqC");
        }
    }
}
