using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddSelectedAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedAnswerId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_SelectedAnswerId",
                table: "GameActions",
                column: "SelectedAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Answers_SelectedAnswerId",
                table: "GameActions",
                column: "SelectedAnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Answers_SelectedAnswerId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_SelectedAnswerId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "SelectedAnswerId",
                table: "GameActions");
        }
    }
}
