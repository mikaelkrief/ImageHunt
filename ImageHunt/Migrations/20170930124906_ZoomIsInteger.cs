using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class ZoomIsInteger : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<int>(
        "MapZoom",
        "Games",
        "int",
        nullable: true,
        oldClrType: typeof(double),
        oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<double>(
        "MapZoom",
        "Games",
        nullable: true,
        oldClrType: typeof(int),
        oldType: "int",
        oldNullable: true);
    }
  }
}
