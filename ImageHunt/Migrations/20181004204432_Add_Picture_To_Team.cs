using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class Add_Picture_To_Team : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "PictureId",
        "Teams",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Teams_PictureId",
        "Teams",
        "PictureId");

      migrationBuilder.AddForeignKey(
        "FK_Teams_Pictures_PictureId",
        "Teams",
        "PictureId",
        "Pictures",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Teams_Pictures_PictureId",
        "Teams");

      migrationBuilder.DropIndex(
        "IX_Teams_PictureId",
        "Teams");

      migrationBuilder.DropColumn(
        "PictureId",
        "Teams");
    }
  }
}
