using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHuntCore.Migrations
{
    public partial class AddObjectNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "PictureNode_IsDeleted",
                table: "Nodes");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Nodes",
                newName: "QuestionNode_Question");

            migrationBuilder.RenameColumn(
                name: "QuestionNode_IsDeleted",
                table: "Nodes",
                newName: "IsDeleted");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Nodes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Nodes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Nodes",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "QuestionNode_Question",
                table: "Nodes",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Nodes",
                newName: "QuestionNode_IsDeleted");

            migrationBuilder.AlterColumn<bool>(
                name: "QuestionNode_IsDeleted",
                table: "Nodes",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Nodes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PictureNode_IsDeleted",
                table: "Nodes",
                nullable: true);
        }
    }
}
