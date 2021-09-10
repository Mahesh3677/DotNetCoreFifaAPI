using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fifa.Data.Migrations
{
    public partial class rank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryClubRankRelation",
                columns: table => new
                {
                    RankId = table.Column<Guid>(nullable: false),
                    CountryID = table.Column<Guid>(nullable: true),
                    ClubID = table.Column<Guid>(nullable: true),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryClubRankRelation", x => x.RankId);
                    table.ForeignKey(
                        name: "FK_CountryClubRankRelation_Club_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryClubRankRelation_Country_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryClubRankRelation_ClubID",
                table: "CountryClubRankRelation",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_CountryClubRankRelation_CountryID",
                table: "CountryClubRankRelation",
                column: "CountryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryClubRankRelation");
        }
    }
}
