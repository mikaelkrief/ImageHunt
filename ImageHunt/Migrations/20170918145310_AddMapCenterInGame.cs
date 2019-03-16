using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddMapCenterInGame : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<double>(
        "MapCenterLat",
        "Games",
        "double",
        nullable: true);

      migrationBuilder.AddColumn<double>(
        "MapCenterLng",
        "Games",
        "double",
        nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "MapCenterLat",
        "Games");

      migrationBuilder.DropColumn(
        "MapCenterLng",
        "Games");
    }
  }
}
