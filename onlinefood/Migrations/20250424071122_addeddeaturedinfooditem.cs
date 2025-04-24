using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlinefood.Migrations
{
    /// <inheritdoc />
    public partial class addeddeaturedinfooditem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "FoodItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "FoodItems");
        }
    }
}
