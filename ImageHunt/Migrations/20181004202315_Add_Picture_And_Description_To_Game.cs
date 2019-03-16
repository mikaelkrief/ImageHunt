using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class Add_Picture_And_Description_To_Game : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        "Description",
        "Games",
        nullable: true);

      migrationBuilder.AddColumn<int>(
        "PictureId",
        "Games",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Games_PictureId",
        "Games",
        "PictureId");

      migrationBuilder.AddForeignKey(
        "FK_Games_Pictures_PictureId",
        "Games",
        "PictureId",
        "Pictures",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Games_Pictures_PictureId",
        "Games");

      migrationBuilder.DropIndex(
        "IX_Games_PictureId",
        "Games");

      migrationBuilder.DropColumn(
        "Description",
        "Games");

      migrationBuilder.DropColumn(
        "PictureId",
        "Games");
    }
  }
}
