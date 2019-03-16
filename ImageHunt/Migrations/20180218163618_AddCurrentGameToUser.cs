using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddCurrentGameToUser : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "CurrentGameId",
        "Players",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Players_CurrentGameId",
        "Players",
        "CurrentGameId");

      migrationBuilder.AddForeignKey(
        "FK_Players_Games_CurrentGameId",
        "Players",
        "CurrentGameId",
        "Games",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Players_Games_CurrentGameId",
        "Players");

      migrationBuilder.DropIndex(
        "IX_Players_CurrentGameId",
        "Players");

      migrationBuilder.DropColumn(
        "CurrentGameId",
        "Players");
    }
  }
}
