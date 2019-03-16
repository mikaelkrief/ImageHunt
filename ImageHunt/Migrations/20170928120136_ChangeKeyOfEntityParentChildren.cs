using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class ChangeKeyOfEntityParentChildren : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable("ParentChildren");

      migrationBuilder.CreateTable(
        "ParentChildren",
        table => new
        {
          ParentId = table.Column<int>("int", nullable: false),
          ChildrenId = table.Column<int>("int", nullable: false),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_ParentChildren", x => x.Id);
          table.UniqueConstraint("AK_ParentChildren_Id", x => x.Id);
          table.ForeignKey(
            "FK_ParentChildren_Nodes_ChildrenId",
            x => x.ChildrenId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_ParentChildren_Nodes_ParentId",
            x => x.ParentId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Cascade);
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}
