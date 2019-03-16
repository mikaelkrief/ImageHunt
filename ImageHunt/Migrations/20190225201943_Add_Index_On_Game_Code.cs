using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class Add_Index_On_Game_Code : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
        "Code",
        "Games",
        nullable: true,
        unicode: false,
        maxLength: 250,
        oldClrType: typeof(string),
        oldNullable: true);

      migrationBuilder.CreateIndex(
        "IX_Games_Code",
        "Games",
        "Code",
        unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
        "IX_Games_Code",
        "Games");

      migrationBuilder.AlterColumn<string>(
        "Code",
        "Games",
        nullable: true,
        oldClrType: typeof(string),
        oldNullable: true);
    }
  }
}
