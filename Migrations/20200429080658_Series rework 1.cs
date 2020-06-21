using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class Seriesrework1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesPicture_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeriesPicture_AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "AnimeSeries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_PictureId",
                table: "AnimeSeries",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesPicture_PictureId",
                table: "AnimeSeries",
                column: "PictureId",
                principalTable: "AnimeSeriesPicture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesPicture_PictureId",
                table: "AnimeSeries");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeries_PictureId",
                table: "AnimeSeries");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "AnimeSeries");

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesPicture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeriesPicture_AnimeSeriesId",
                table: "AnimeSeriesPicture",
                column: "AnimeSeriesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesPicture_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesPicture",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
