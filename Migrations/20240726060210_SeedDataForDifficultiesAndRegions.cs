using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForDifficultiesAndRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"), "Hard" },
                    { new Guid("b084bc9e-da20-43c7-831f-56fa43eab106"), "Medium" },
                    { new Guid("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("919276a6-b956-41f5-856c-f27653292008"), "AKL", "New Zealand", "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Auckland_City_Skyline.jpg/1200px-Auckland_City_Skyline.jpg" },
                    { new Guid("919276a6-b956-41f5-856c-f27653292009"), "QLD", "Queensland", "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Brisbane_CITY_Harbour_and_CBD.jpg/1200px-Brisbane_CITY_Harbour_and_CBD.jpg" },
                    { new Guid("919276a6-b956-41f5-856c-f27653292010"), "VIC", "Victoria", "https://upload.wikimedia.org/wikipedia/commons/thumb/0/01/Melbourne_City_Skyline.jpg/1200px-Melbourne_City_Skyline.jpg" },
                    { new Guid("919276a6-b956-41f5-856c-f27653292011"), "TAS", "Tasmania", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/Hobart_City_Skyline.jpg/1200px-Hobart_City_Skyline.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b084bc9e-da20-43c7-831f-56fa43eab106"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292008"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292009"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292010"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292011"));
        }
    }
}
