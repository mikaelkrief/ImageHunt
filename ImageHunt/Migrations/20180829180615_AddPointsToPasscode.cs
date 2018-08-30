using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddPointsToPasscode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Passcodes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Passcodes");
        }
    }
}
