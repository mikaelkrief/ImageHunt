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
          name: "FK_Answers_Nodes_QuestionNodeId",
          table: "Answers");

      migrationBuilder.RenameColumn(
                name: "QuestionNodeId",
                table: "Answers",
                newName: "ChoiceNodeId");

      migrationBuilder.DropIndex("IX_Answers_QuestionNodeId", table: "Answers");
      migrationBuilder.CreateIndex(
        name: "IX_Answers_ChoiceNodeId",
        table: "Answers",
        column: "ChoiceNodeId");


      migrationBuilder.AddForeignKey(
          name: "FK_Answers_Nodes_ChoiceNodeId",
          table: "Answers",
          column: "ChoiceNodeId",
          principalTable: "Nodes",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Answers_Nodes_ChoiceNodeId",
          table: "Answers");

      migrationBuilder.RenameColumn(
          name: "ChoiceNodeId",
          table: "Answers",
          newName: "QuestionNodeId");

      migrationBuilder.RenameIndex(
          name: "IX_Answers_ChoiceNodeId",
          table: "Answers",
          newName: "IX_Answers_QuestionNodeId");

      migrationBuilder.AddForeignKey(
          name: "FK_Answers_Nodes_QuestionNodeId",
          table: "Answers",
          column: "QuestionNodeId",
          principalTable: "Nodes",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
