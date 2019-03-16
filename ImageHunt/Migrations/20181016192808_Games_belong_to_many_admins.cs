using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class Games_belong_to_many_admins : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Games_Admins_AdminId",
        "Games");

      migrationBuilder.DropIndex(
        "IX_Games_AdminId",
        "Games");

      migrationBuilder.DropColumn(
        "AdminId",
        "Games");

      migrationBuilder.CreateTable(
        "GameAdmin",
        table => new
        {
          GameId = table.Column<int>(nullable: false),
          AdminId = table.Column<int>(nullable: false),
          IsDeleted = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_GameAdmin", x => new {x.AdminId, x.GameId});
          table.ForeignKey(
            "FK_GameAdmin_Admins_AdminId",
            x => x.AdminId,
            "Admins",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_GameAdmin_Games_GameId",
            x => x.GameId,
            "Games",
            "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        "IX_GameAdmin_GameId",
        "GameAdmin",
        "GameId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "GameAdmin");

      migrationBuilder.AddColumn<int>(
        "AdminId",
        "Games",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Games_AdminId",
        "Games",
        "AdminId");

      migrationBuilder.AddForeignKey(
        "FK_Games_Admins_AdminId",
        "Games",
        "AdminId",
        "Admins",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }
  }
}
