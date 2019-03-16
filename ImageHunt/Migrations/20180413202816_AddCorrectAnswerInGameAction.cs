using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddCorrectAnswerInGameAction : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "CorrectAnswerId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_GameActions_CorrectAnswerId",
        "GameActions",
        "CorrectAnswerId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Answers_CorrectAnswerId",
        "GameActions",
        "CorrectAnswerId",
        "Answers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Answers_CorrectAnswerId",
        "GameActions");

      migrationBuilder.DropIndex(
        "IX_GameActions_CorrectAnswerId",
        "GameActions");

      migrationBuilder.DropColumn(
        "CorrectAnswerId",
        "GameActions");
    }
  }
}
