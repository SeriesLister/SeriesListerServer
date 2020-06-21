using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class SE2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.AlterColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
