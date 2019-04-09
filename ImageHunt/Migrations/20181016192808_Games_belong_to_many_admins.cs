using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class GamesBelongToManyAdmins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
      migrationBuilder.DropForeignKey(
          name: "FK_Games_Admins_AdminId",
          table: "Games");

      migrationBuilder.DropIndex(
          name: "IX_Games_AdminId",
          table: "Games");

      migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameAdmin",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAdmin", x => new { x.AdminId, x.GameId });
                    table.ForeignKey(
                        name: "FK_GameAdmin_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameAdmin_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameAdmin_GameId",
                table: "GameAdmin",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameAdmin");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_AdminId",
                table: "Games",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Admins_AdminId",
                table: "Games",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
