using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImageHunt.Migrations
{
    public partial class ChangeKeyOfEntityParentChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ParentChildren_Id",
                table: "ParentChildren");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentChildren",
                table: "ParentChildren");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentChildren",
                table: "ParentChildren",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildren_ParentId",
                table: "ParentChildren",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentChildren",
                table: "ParentChildren");

            migrationBuilder.DropIndex(
                name: "IX_ParentChildren_ParentId",
                table: "ParentChildren");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ParentChildren_Id",
                table: "ParentChildren",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentChildren",
                table: "ParentChildren",
                columns: new[] { "ParentId", "ChildrenId" });
        }
    }
}
