using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddHiddenNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationHint",
                table: "Nodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationHint",
                table: "Nodes");
        }
    }
}
