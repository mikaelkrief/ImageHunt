using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddTeamInGameAction : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "CurrentNodeId",
        "Teams",
        nullable: true);

      migrationBuilder.AddColumn<int>(
        "TeamId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Teams_CurrentNodeId",
        "Teams",
        "CurrentNodeId");

      migrationBuilder.CreateIndex(
        "IX_GameActions_TeamId",
        "GameActions",
        "TeamId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Teams_TeamId",
        "GameActions",
        "TeamId",
        "Teams",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
        "FK_Teams_Nodes_CurrentNodeId",
        "Teams",
        "CurrentNodeId",
        "Nodes",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Teams_TeamId",
        "GameActions");

      migrationBuilder.DropForeignKey(
        "FK_Teams_Nodes_CurrentNodeId",
        "Teams");

      migrationBuilder.DropIndex(
        "IX_Teams_CurrentNodeId",
        "Teams");

      migrationBuilder.DropIndex(
        "IX_GameActions_TeamId",
        "GameActions");

      migrationBuilder.DropColumn(
        "CurrentNodeId",
        "Teams");

      migrationBuilder.DropColumn(
        "TeamId",
        "GameActions");
    }
  }
}
