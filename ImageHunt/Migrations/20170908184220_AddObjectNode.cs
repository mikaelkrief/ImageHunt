using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddObjectNode : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "IsDeleted",
        "Nodes");

      migrationBuilder.DropColumn(
        "PictureNode_IsDeleted",
        "Nodes");

      migrationBuilder.RenameColumn(
        "Question",
        "Nodes",
        "QuestionNode_Question");

      migrationBuilder.RenameColumn(
        "QuestionNode_IsDeleted",
        "Nodes",
        "IsDeleted");

      migrationBuilder.AlterColumn<bool>(
        "IsDeleted",
        "Nodes",
        "bit",
        nullable: false,
        oldClrType: typeof(bool),
        oldNullable: true);

      migrationBuilder.AddColumn<string>(
        "Question",
        "Nodes",
        "longtext",
        nullable: true);

      migrationBuilder.AddColumn<byte[]>(
        "Image",
        "Nodes",
        "longblob",
        nullable: true);

      migrationBuilder.AddColumn<bool>(
        "IsActive",
        "Games",
        "bit",
        nullable: false,
        defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "Question",
        "Nodes");

      migrationBuilder.DropColumn(
        "Image",
        "Nodes");

      migrationBuilder.DropColumn(
        "IsActive",
        "Games");

      migrationBuilder.RenameColumn(
        "QuestionNode_Question",
        "Nodes",
        "Question");

      migrationBuilder.RenameColumn(
        "IsDeleted",
        "Nodes",
        "QuestionNode_IsDeleted");

      migrationBuilder.AlterColumn<bool>(
        "QuestionNode_IsDeleted",
        "Nodes",
        nullable: true,
        oldClrType: typeof(bool),
        oldType: "bit");

      migrationBuilder.AddColumn<bool>(
        "IsDeleted",
        "Nodes",
        nullable: true);

      migrationBuilder.AddColumn<bool>(
        "PictureNode_IsDeleted",
        "Nodes",
        nullable: true);
    }
  }
}
