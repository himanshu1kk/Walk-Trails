using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class Hello5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Disliked",
                table: "Attractions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Favorites",
                table: "Attractions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTip",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Attractions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disliked",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "StudentTip",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Attractions");
        }
    }
}
