using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class ChangeNameChoiceNode : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Answers_Nodes_QuestionNodeId",
        "Answers");

      migrationBuilder.RenameColumn(
        "QuestionNodeId",
        "Answers",
        "ChoiceNodeId");

      migrationBuilder.DropIndex("IX_Answers_QuestionNodeId", "Answers");
      migrationBuilder.CreateIndex(
        "IX_Answers_ChoiceNodeId",
        "Answers",
        "ChoiceNodeId");


      migrationBuilder.AddForeignKey(
        "FK_Answers_Nodes_ChoiceNodeId",
        "Answers",
        "ChoiceNodeId",
        "Nodes",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Answers_Nodes_ChoiceNodeId",
        "Answers");

      migrationBuilder.RenameColumn(
        "ChoiceNodeId",
        "Answers",
        "QuestionNodeId");

      migrationBuilder.RenameIndex(
        "IX_Answers_ChoiceNodeId",
        table: "Answers",
        newName: "IX_Answers_QuestionNodeId");

      migrationBuilder.AddForeignKey(
        "FK_Answers_Nodes_QuestionNodeId",
        "Answers",
        "QuestionNodeId",
        "Nodes",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }
  }
}
