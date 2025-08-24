using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class Hello1111111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommnetsOnAttraction",
                columns: table => new
                {
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommnetsOnAttraction", x => x.CommentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommnetsOnAttraction");
        }
    }
}
