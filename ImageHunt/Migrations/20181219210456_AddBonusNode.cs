using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddBonusNode : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
        "BonusType",
        "Nodes",
        nullable: true);

      migrationBuilder.AddColumn<string>(
        "Location",
        "Nodes",
        nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "BonusType",
        "Nodes");

      migrationBuilder.DropColumn(
        "Location",
        "Nodes");
    }
  }
}
