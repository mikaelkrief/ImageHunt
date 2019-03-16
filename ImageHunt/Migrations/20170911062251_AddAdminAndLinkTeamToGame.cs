using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddAdminAndLinkTeamToGame : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "GameId",
        "Teams",
        "int",
        nullable: true);

      migrationBuilder.AddColumn<int>(
        "AdminId",
        "Games",
        "int",
        nullable: true);

      migrationBuilder.CreateTable(
        "Admins",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Email = table.Column<string>("longtext", nullable: true),
          ExpirationTokenDate = table.Column<DateTime>("datetime(6)", nullable: false),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Name = table.Column<string>("longtext", nullable: true),
          Token = table.Column<string>("longtext", nullable: true)
        },
        constraints: table => { table.PrimaryKey("PK_Admins", x => x.Id); });

      migrationBuilder.CreateIndex(
        "IX_Teams_GameId",
        "Teams",
        "GameId");

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

      migrationBuilder.AddForeignKey(
        "FK_Teams_Games_GameId",
        "Teams",
        "GameId",
        "Games",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Games_Admins_AdminId",
        "Games");

      migrationBuilder.DropForeignKey(
        "FK_Teams_Games_GameId",
        "Teams");

      migrationBuilder.DropTable(
        "Admins");

      migrationBuilder.DropIndex(
        "IX_Teams_GameId",
        "Teams");

      migrationBuilder.DropIndex(
        "IX_Games_AdminId",
        "Games");

      migrationBuilder.DropColumn(
        "GameId",
        "Teams");

      migrationBuilder.DropColumn(
        "AdminId",
        "Games");
    }
  }
}
