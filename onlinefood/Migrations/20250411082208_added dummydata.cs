using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlinefood.Migrations
{
    /// <inheritdoc />
    public partial class addeddummydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsVerified", "Name", "Password", "Role" },
                values: new object[] { 1, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), "admin@admin.com", true, "Admin", "admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
