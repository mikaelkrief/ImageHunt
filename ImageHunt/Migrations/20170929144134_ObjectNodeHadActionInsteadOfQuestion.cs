using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class ObjectNodeHadActionInsteadOfQuestion : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "Question",
        "Nodes");

      migrationBuilder.RenameColumn(
        "QuestionNode_Question",
        "Nodes",
        "Question");

      migrationBuilder.AddColumn<string>(
        "Action",
        "Nodes",
        "longtext",
        nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "Action",
        "Nodes");

      migrationBuilder.RenameColumn(
        "Question",
        "Nodes",
        "QuestionNode_Question");

      migrationBuilder.AddColumn<string>(
        "Question",
        "Nodes",
        nullable: true);
    }
  }
}
