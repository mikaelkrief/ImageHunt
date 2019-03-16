using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class RemoveIsDeletedForGameAdminEntity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "IsDeleted",
        "GameAdmin");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
        "IsDeleted",
        "GameAdmin",
        nullable: false,
        defaultValue: false);
    }
  }
}
