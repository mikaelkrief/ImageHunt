using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
    public partial class AddIndexOnGameCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Games",
                nullable: true,
                unicode: false,
                maxLength:250,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Code",
                table: "Games",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Code",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Games",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
