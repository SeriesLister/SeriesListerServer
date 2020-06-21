using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class SE3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.AlterColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.AlterColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
