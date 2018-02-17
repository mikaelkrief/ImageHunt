using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHuntCore.Migrations
{
    public partial class AddPictureEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Nodes");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Nodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<byte[]>(type: "longblob", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ImageId",
                table: "Nodes",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Pictures_ImageId",
                table: "Nodes",
                column: "ImageId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Pictures_ImageId",
                table: "Nodes");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Nodes_ImageId",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Nodes");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Nodes",
                nullable: true);
        }
    }
}
