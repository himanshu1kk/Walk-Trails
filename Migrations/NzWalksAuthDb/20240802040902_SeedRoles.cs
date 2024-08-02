using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NzWalks.Migrations.NzWalksAuthDb
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "readerRoleId");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "writerRoleId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07c02303-1e8f-4b7f-a258-f47c065be550", "07c02303-1e8f-4b7f-a258-f47c065be550", "Writer", "WRITER" },
                    { "22181a3d-3a16-462d-9f47-8ccbbe1c6691", "22181a3d-3a16-462d-9f47-8ccbbe1c6691", "Reader", "READER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07c02303-1e8f-4b7f-a258-f47c065be550");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22181a3d-3a16-462d-9f47-8ccbbe1c6691");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "readerRoleId", "22181a3d-3a16-462d-9f47-8ccbbe1c6691", "Reader", "Reader.ToUpper" },
                    { "writerRoleId", "07c02303-1e8f-4b7f-a258-f47c065be550", "Writer", "Writer.ToUpper" }
                });
        }
    }
}
