using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTablessasss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DifficultyId1",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "RegionId1",
                table: "Walks");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId1",
                table: "Walks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Walks_DifficultyId1",
                table: "Walks",
                column: "DifficultyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_RegionId1",
                table: "Walks",
                column: "RegionId1");

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
    }
}
