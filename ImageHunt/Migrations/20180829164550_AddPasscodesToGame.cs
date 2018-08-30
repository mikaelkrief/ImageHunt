using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddPasscodesToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Passcodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passcodes_GameId",
                table: "Passcodes",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passcodes_Games_GameId",
                table: "Passcodes",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passcodes_Games_GameId",
                table: "Passcodes");

            migrationBuilder.DropIndex(
                name: "IX_Passcodes_GameId",
                table: "Passcodes");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Passcodes");
        }
    }
}
