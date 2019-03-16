using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddPlayerPenalty : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "NbPlayerPenaltyThreshold",
        "Games",
        nullable: false,
        defaultValue: 3);

      migrationBuilder.AddColumn<double>(
        "NbPlayerPenaltyValue",
        "Games",
        nullable: false,
        defaultValue: 0.05);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "NbPlayerPenaltyThreshold",
        "Games");

      migrationBuilder.DropColumn(
        "NbPlayerPenaltyValue",
        "Games");
    }
  }
}
