using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddTeamPlayerEntity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Players_Teams_TeamId",
        "Players");
      migrationBuilder.DropIndex(
        "IX_Players_TeamId",
        "Players"
      );
      migrationBuilder.DropColumn(
        "TeamId",
        "Players");

      migrationBuilder.CreateTable(
        "TeamPlayer",
        table => new
        {
          TeamId = table.Column<int>(nullable: false),
          PlayerId = table.Column<int>(nullable: false),
          IsDeleted = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_TeamPlayer", x => new {x.TeamId, x.PlayerId});
          table.ForeignKey(
            "FK_TeamPlayer_Players_PlayerId",
            x => x.PlayerId,
            "Players",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_TeamPlayer_Teams_TeamId",
            x => x.TeamId,
            "Teams",
            "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        "IX_TeamPlayer_PlayerId",
        "TeamPlayer",
        "PlayerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "TeamPlayer");


      migrationBuilder.AddColumn<int>(
        "TeamId",
        "Players",
        nullable: true);
    }
  }
}
