using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddIsDeletedPropertyToParentChildrenEntity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
        "IsDeleted",
        "ParentChildren",
        "bit",
        nullable: false,
        defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "IsDeleted",
        "ParentChildren");
    }
  }
}
