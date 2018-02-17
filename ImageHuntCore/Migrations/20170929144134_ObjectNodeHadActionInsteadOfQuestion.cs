using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHuntCore.Migrations
{
    public partial class ObjectNodeHadActionInsteadOfQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "Nodes");

            migrationBuilder.RenameColumn(
                name: "QuestionNode_Question",
                table: "Nodes",
                newName: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Nodes",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Nodes");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Nodes",
                newName: "QuestionNode_Question");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Nodes",
                nullable: true);
        }
    }
}
