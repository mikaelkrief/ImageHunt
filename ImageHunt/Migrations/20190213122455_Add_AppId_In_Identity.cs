using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class Add_AppId_In_Identity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          "AppUserId",
          "AspNetUsers",
          nullable: false,
          defaultValue: 0)
        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "AppUserId",
        "AspNetUsers");
    }
  }
}
