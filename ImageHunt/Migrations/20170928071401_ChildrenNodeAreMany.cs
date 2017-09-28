using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImageHunt.Migrations
{
    public partial class ChildrenNodeAreMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Nodes_NodeId",
                table: "Nodes");

            migrationBuilder.DropIndex(
                name: "IX_Nodes_NodeId",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "NodeId",
                table: "Nodes");

            migrationBuilder.CreateTable(
                name: "ParentChildren",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildrenId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChildren", x => new { x.ParentId, x.ChildrenId });
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

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildren_ChildrenId",
                table: "ParentChildren",
                column: "ChildrenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentChildren");

            migrationBuilder.AddColumn<int>(
                name: "NodeId",
                table: "Nodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_NodeId",
                table: "Nodes",
                column: "NodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Nodes_NodeId",
                table: "Nodes",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
