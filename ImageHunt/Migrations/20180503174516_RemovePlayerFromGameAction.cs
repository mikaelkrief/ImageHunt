using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class RemovePlayerFromGameAction : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Players_PlayerId",
        "GameActions");

      migrationBuilder.DropIndex(
        "IX_GameActions_PlayerId",
        "GameActions");

      migrationBuilder.DropColumn(
        "PlayerId",
        "GameActions");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "PlayerId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_GameActions_PlayerId",
        "GameActions",
        "PlayerId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Players_PlayerId",
        "GameActions",
        "PlayerId",
        "Players",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }
  }
}
