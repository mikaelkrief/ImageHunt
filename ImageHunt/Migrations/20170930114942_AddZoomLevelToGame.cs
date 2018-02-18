using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddZoomLevelToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MapZoom",
                table: "Games",
                type: "double",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapZoom",
                table: "Games");
        }
    }
}
