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
                name: "BonusType",
                table: "Nodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Nodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusType",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Nodes");
        }
    }
}
