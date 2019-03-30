using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddTeamPlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name:"FK_Players_Teams_TeamId", 
                table:"Players");
            migrationBuilder.DropIndex(
                name:"IX_Players_TeamId",
                table:"Players"
            );                
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "TeamPlayer",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => new { x.TeamId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayer_PlayerId",
                table: "TeamPlayer",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPlayer");


            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Players",
                nullable: true);



        }
    }
}
