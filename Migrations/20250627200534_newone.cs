using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalks.Migrations
{
    /// <inheritdoc />
    public partial class newone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttractionImages_Attractions_AttractionsModelId",
                table: "AttractionImages");

            migrationBuilder.DropTable(
                name: "LocationInfos");

            migrationBuilder.RenameColumn(
                name: "Suggestion",
                table: "Attractions",
                newName: "Suggestions");

            migrationBuilder.RenameColumn(
                name: "RatingsByAdmin",
                table: "Attractions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "NotToMissOut",
                table: "Attractions",
                newName: "NotToMiss");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Attractions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "AttractionType",
                table: "Attractions",
                newName: "AdminRating");

            migrationBuilder.RenameColumn(
                name: "AttractionName",
                table: "Attractions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AttractionCoverImageUrl",
                table: "Attractions",
                newName: "CoverImageUrl");

            migrationBuilder.RenameColumn(
                name: "AttractionsModelId",
                table: "AttractionImages",
                newName: "AttractionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttractionImages_AttractionsModelId",
                table: "AttractionImages",
                newName: "IX_AttractionImages_AttractionId");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AttractionId",
                table: "Locations",
                column: "AttractionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Attractions",
                newName: "RatingsByAdmin");

            migrationBuilder.RenameColumn(
                name: "Suggestions",
                table: "Attractions",
                newName: "Suggestion");

            migrationBuilder.RenameColumn(
                name: "NotToMiss",
                table: "Attractions",
                newName: "NotToMissOut");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Attractions",
                newName: "AttractionName");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Attractions",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Attractions",
                newName: "AttractionCoverImageUrl");

            migrationBuilder.RenameColumn(
                name: "AdminRating",
                table: "Attractions",
                newName: "AttractionType");

            migrationBuilder.RenameColumn(
                name: "AttractionId",
                table: "AttractionImages",
                newName: "AttractionsModelId");

            migrationBuilder.RenameIndex(
                name: "IX_AttractionImages_AttractionId",
                table: "AttractionImages",
                newName: "IX_AttractionImages_AttractionsModelId");

            migrationBuilder.CreateTable(
                name: "LocationInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttractionsModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationInfos_Attractions_AttractionsModelId",
                        column: x => x.AttractionsModelId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInfos_AttractionsModelId",
                table: "LocationInfos",
                column: "AttractionsModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionImages_Attractions_AttractionsModelId",
                table: "AttractionImages",
                column: "AttractionsModelId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
