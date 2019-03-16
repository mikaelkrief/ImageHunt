using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class GameAction_Geocoordinates_can_be_null : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<double>(
        "Longitude",
        "GameActions",
        nullable: true,
        oldClrType: typeof(double));

      migrationBuilder.AlterColumn<double>(
        "Latitude",
        "GameActions",
        nullable: true,
        oldClrType: typeof(double));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<double>(
        "Longitude",
        "GameActions",
        nullable: false,
        oldClrType: typeof(double),
        oldNullable: true);

      migrationBuilder.AlterColumn<double>(
        "Latitude",
        "GameActions",
        nullable: false,
        oldClrType: typeof(double),
        oldNullable: true);
    }
  }
}
