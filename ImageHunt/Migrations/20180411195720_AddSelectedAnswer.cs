using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddSelectedAnswer : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "SelectedAnswerId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_GameActions_SelectedAnswerId",
        "GameActions",
        "SelectedAnswerId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Answers_SelectedAnswerId",
        "GameActions",
        "SelectedAnswerId",
        "Answers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Answers_SelectedAnswerId",
        "GameActions");

      migrationBuilder.DropIndex(
        "IX_GameActions_SelectedAnswerId",
        "GameActions");

      migrationBuilder.DropColumn(
        "SelectedAnswerId",
        "GameActions");
    }
  }
}
