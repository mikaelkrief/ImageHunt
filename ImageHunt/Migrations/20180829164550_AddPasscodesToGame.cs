using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddPasscodesToGame : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "GameId",
        "Passcodes",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Passcodes_GameId",
        "Passcodes",
        "GameId");

      migrationBuilder.AddForeignKey(
        "FK_Passcodes_Games_GameId",
        "Passcodes",
        "GameId",
        "Games",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Passcodes_Games_GameId",
        "Passcodes");

      migrationBuilder.DropIndex(
        "IX_Passcodes_GameId",
        "Passcodes");

      migrationBuilder.DropColumn(
        "GameId",
        "Passcodes");
    }
  }
}
