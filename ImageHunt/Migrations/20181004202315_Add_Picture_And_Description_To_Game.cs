using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddPictureAndDescriptionToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PictureId",
                table: "Games",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Pictures_PictureId",
                table: "Games",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Pictures_PictureId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_PictureId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Games");
        }
    }
}
