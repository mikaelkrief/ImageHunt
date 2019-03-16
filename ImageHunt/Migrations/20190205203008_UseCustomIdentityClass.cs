using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class UseCustomIdentityClass : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        "TelegramUser",
        "AspNetUsers",
        nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "TelegramUser",
        "AspNetUsers");
    }
  }
}
