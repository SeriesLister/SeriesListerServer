using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class AnimeSeriesES1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesSE_AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesPicture_PictureId",
                table: "AnimeSeries");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeries_AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeries_PictureId",
                table: "AnimeSeries");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "AnimeSeries");

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesPicture",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeriesSE_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeriesPicture_AnimeSeriesId",
                table: "AnimeSeriesPicture",
                column: "AnimeSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesPicture_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesPicture",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesPicture_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeriesSE_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeriesPicture_AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesId",
                table: "AnimeSeriesPicture");

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesSEId",
                table: "AnimeSeries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "AnimeSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_AnimeSeriesSEId",
                table: "AnimeSeries",
                column: "AnimeSeriesSEId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_PictureId",
                table: "AnimeSeries",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesSE_AnimeSeriesSEId",
                table: "AnimeSeries",
                column: "AnimeSeriesSEId",
                principalTable: "AnimeSeriesSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesPicture_PictureId",
                table: "AnimeSeries",
                column: "PictureId",
                principalTable: "AnimeSeriesPicture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
