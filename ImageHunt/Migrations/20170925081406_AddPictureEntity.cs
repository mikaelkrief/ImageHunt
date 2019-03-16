using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddPictureEntity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "Image",
        "Nodes");

      migrationBuilder.AddColumn<int>(
        "ImageId",
        "Nodes",
        "int",
        nullable: true);

      migrationBuilder.CreateTable(
        "Pictures",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Image = table.Column<byte[]>("longblob", nullable: true),
          IsDeleted = table.Column<bool>("bit", nullable: false)
        },
        constraints: table => { table.PrimaryKey("PK_Pictures", x => x.Id); });

      migrationBuilder.CreateIndex(
        "IX_Nodes_ImageId",
        "Nodes",
        "ImageId");

      migrationBuilder.AddForeignKey(
        "FK_Nodes_Pictures_ImageId",
        "Nodes",
        "ImageId",
        "Pictures",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_Nodes_Pictures_ImageId",
        "Nodes");

      migrationBuilder.DropTable(
        "Pictures");

      migrationBuilder.DropIndex(
        "IX_Nodes_ImageId",
        "Nodes");

      migrationBuilder.DropColumn(
        "ImageId",
        "Nodes");

      migrationBuilder.AddColumn<byte[]>(
        "Image",
        "Nodes",
        nullable: true);
    }
  }
}
