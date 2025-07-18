using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class Hello7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpvotedByUsers",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpvotedByUsers",
                table: "Attractions");
        }
    }
}
