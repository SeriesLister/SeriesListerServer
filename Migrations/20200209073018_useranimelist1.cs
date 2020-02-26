using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeListings.Migrations
{
    public partial class useranimelist1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAnimeLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    AnimeSeriesId = table.Column<int>(nullable: true),
                    CurrentEpisode = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnimeLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnimeLists_AnimeSeries_AnimeSeriesId",
                        column: x => x.AnimeSeriesId,
                        principalTable: "AnimeSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAnimeLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimeLists_AnimeSeriesId",
                table: "UserAnimeLists",
                column: "AnimeSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimeLists_UserId",
                table: "UserAnimeLists",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnimeLists");
        }
    }
}
