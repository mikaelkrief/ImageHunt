using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddChatLoginForPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatLogin",
                table: "Players",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatLogin",
                table: "Players");
        }
    }
}
