using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddPasscode : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        "Passcodes",
        table => new
        {
          Id = table.Column<int>(nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Pass = table.Column<string>(nullable: true),
          IsDeleted = table.Column<bool>(nullable: false)
        },
        constraints: table => { table.PrimaryKey("PK_Passcodes", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "Passcodes");
    }
  }
}
