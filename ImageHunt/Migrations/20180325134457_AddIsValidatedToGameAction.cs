using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddIsValidatedToGameAction : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
        "IsValidated",
        "GameActions",
        nullable: false,
        defaultValue: false);

      migrationBuilder.AddColumn<int>(
        "NodeId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_GameActions_NodeId",
        "GameActions",
        "NodeId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Nodes_NodeId",
        "GameActions",
        "NodeId",
        "Nodes",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Nodes_NodeId",
        "GameActions");

      migrationBuilder.DropIndex(
        "IX_GameActions_NodeId",
        "GameActions");

      migrationBuilder.DropColumn(
        "IsValidated",
        "GameActions");

      migrationBuilder.DropColumn(
        "NodeId",
        "GameActions");
    }
  }
}
