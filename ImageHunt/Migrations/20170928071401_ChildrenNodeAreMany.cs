using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class ChildrenNodeAreMany : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Nodes_Nodes_NodeId",
        "Nodes");

      migrationBuilder.DropIndex(
        "IX_Nodes_NodeId",
        "Nodes");

      migrationBuilder.DropColumn(
        "NodeId",
        "Nodes");

      migrationBuilder.CreateTable(
        "ParentChildren",
        table => new
        {
          ParentId = table.Column<int>("int", nullable: false),
          ChildrenId = table.Column<int>("int", nullable: false),
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_ParentChildren", x => new {x.ParentId, x.ChildrenId});
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

      migrationBuilder.CreateIndex(
        "IX_ParentChildren_ChildrenId",
        "ParentChildren",
        "ChildrenId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "ParentChildren");

      migrationBuilder.AddColumn<int>(
        "NodeId",
        "Nodes",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_Nodes_NodeId",
        "Nodes",
        "NodeId");

      migrationBuilder.AddForeignKey(
        "FK_Nodes_Nodes_NodeId",
        "Nodes",
        "NodeId",
        "Nodes",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }
  }
}
