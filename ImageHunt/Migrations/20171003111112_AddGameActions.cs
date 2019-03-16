using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddGameActions : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        "GameActions",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          DateOccured = table.Column<DateTime>("datetime(6)", nullable: false),
          GameId = table.Column<int>("int", nullable: true),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Latitude = table.Column<double>("double", nullable: false),
          Longitude = table.Column<double>("double", nullable: false),
          PictureId = table.Column<int>("int", nullable: true),
          PlayerId = table.Column<int>("int", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_GameActions", x => x.Id);
          table.ForeignKey(
            "FK_GameActions_Games_GameId",
            x => x.GameId,
            "Games",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_GameActions_Pictures_PictureId",
            x => x.PictureId,
            "Pictures",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_GameActions_Players_PlayerId",
            x => x.PlayerId,
            "Players",
            "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateIndex(
        "IX_GameActions_GameId",
        "GameActions",
        "GameId");

      migrationBuilder.CreateIndex(
        "IX_GameActions_PictureId",
        "GameActions",
        "PictureId");

      migrationBuilder.CreateIndex(
        "IX_GameActions_PlayerId",
        "GameActions",
        "PlayerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "GameActions");
    }
  }
}
