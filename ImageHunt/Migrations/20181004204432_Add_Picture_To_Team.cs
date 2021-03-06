using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddPictureToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_PictureId",
                table: "Teams",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Pictures_PictureId",
                table: "Teams",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Pictures_PictureId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_PictureId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Teams");
        }
    }
}
