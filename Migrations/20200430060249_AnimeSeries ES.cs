using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class AnimeSeriesES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpisodesS",
                table: "AnimeSeries");

            migrationBuilder.AddColumn<int>(
                name: "AnimeSeriesSEId",
                table: "AnimeSeries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnimeSeriesSE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Season = table.Column<int>(nullable: false),
                    Episodes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSeriesSE", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_AnimeSeriesSEId",
                table: "AnimeSeries",
                column: "AnimeSeriesSEId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesSE_AnimeSeriesSEId",
                table: "AnimeSeries",
                column: "AnimeSeriesSEId",
                principalTable: "AnimeSeriesSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeries_AnimeSeriesSE_AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.DropTable(
                name: "AnimeSeriesSE");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeries_AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.DropColumn(
                name: "AnimeSeriesSEId",
                table: "AnimeSeries");

            migrationBuilder.AddColumn<string>(
                name: "EpisodesS",
                table: "AnimeSeries",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
