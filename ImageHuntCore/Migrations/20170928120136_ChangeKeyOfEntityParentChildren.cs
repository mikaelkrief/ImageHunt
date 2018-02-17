using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHuntCore.Migrations
{
    public partial class ChangeKeyOfEntityParentChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.DropTable("ParentChildren");

      migrationBuilder.CreateTable(
        name: "ParentChildren",
        columns: table => new
        {
          ParentId = table.Column<int>(type: "int", nullable: false),
          ChildrenId = table.Column<int>(type: "int", nullable: false),
          IsDeleted = table.Column<bool>(type:"bit", nullable:false),
          Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_ParentChildren", x => x.Id);
          table.UniqueConstraint("AK_ParentChildren_Id", x => x.Id);
          table.ForeignKey(
            name: "FK_ParentChildren_Nodes_ChildrenId",
            column: x => x.ChildrenId,
            principalTable: "Nodes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            name: "FK_ParentChildren_Nodes_ParentId",
            column: x => x.ParentId,
            principalTable: "Nodes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        });

    }

    protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
