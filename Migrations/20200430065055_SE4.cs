using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class SE4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeriesSE_AnimeSeries_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeriesSE_AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AnimeSeriesSE",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_SE_AnimeSeries",
                table: "AnimeSeriesSE",
                column: "Id",
                principalTable: "AnimeSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_SE_AnimeSeries",
                table: "AnimeSeriesSE");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AnimeSeriesSE",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesId",
                table: "AnimeSeriesSE",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeriesSE_AnimeSeriesId",
                table: "AnimeSeriesSE",
                column: "AnimeSeriesId");

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
