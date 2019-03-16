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
                name: "NbPlayerPenaltyThreshold",
                table: "Games",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<double>(
                name: "NbPlayerPenaltyValue",
                table: "Games",
                nullable: false,
                defaultValue: 0.05);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NbPlayerPenaltyThreshold",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NbPlayerPenaltyValue",
                table: "Games");
        }
    }
}
