using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId",
                table: "Walks");

            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Regions_RegionId",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_DifficultyId",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_RegionId",
                table: "Walks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "RegionId",
                table: "Walks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "DifficultyId",
                table: "Walks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyId1",
                table: "Walks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EstimatedDurationHours",
                table: "Walks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId1",
                table: "Walks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PointsAdded",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserImageUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalRegionName",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Toughness",
                table: "Difficulties",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"),
                column: "Toughness",
                value: null);

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b084bc9e-da20-43c7-831f-56fa43eab106"),
                column: "Toughness",
                value: null);

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"),
                column: "Toughness",
                value: null);

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292008"),
                columns: new[] { "CountryName", "DistrictName", "LocalRegionName", "PinCode", "StateName" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292009"),
                columns: new[] { "CountryName", "DistrictName", "LocalRegionName", "PinCode", "StateName" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292010"),
                columns: new[] { "CountryName", "DistrictName", "LocalRegionName", "PinCode", "StateName" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("919276a6-b956-41f5-856c-f27653292011"),
                columns: new[] { "CountryName", "DistrictName", "LocalRegionName", "PinCode", "StateName" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Walks_DifficultyId1",
                table: "Walks",
                column: "DifficultyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_RegionId1",
                table: "Walks",
                column: "RegionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId1",
                table: "Walks",
                column: "DifficultyId1",
                principalTable: "Difficulties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Regions_RegionId1",
                table: "Walks",
                column: "RegionId1",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId1",
                table: "Walks");

            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Regions_RegionId1",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_DifficultyId1",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_RegionId1",
                table: "Walks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DifficultyId1",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "EstimatedDurationHours",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "RegionId1",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PointsAdded",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserImageUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "LocalRegionName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "StateName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Toughness",
                table: "Difficulties");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Walks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DifficultyId",
                table: "Walks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_DifficultyId",
                table: "Walks",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_RegionId",
                table: "Walks",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Difficulties_DifficultyId",
                table: "Walks",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Regions_RegionId",
                table: "Walks",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
