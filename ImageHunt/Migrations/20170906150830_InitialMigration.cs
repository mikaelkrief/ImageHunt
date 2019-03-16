using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class InitialMigration : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        "Games",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Name = table.Column<string>("longtext", nullable: true),
          StartDate = table.Column<DateTime>("datetime(6)", nullable: false)
        },
        constraints: table => { table.PrimaryKey("PK_Games", x => x.Id); });

      migrationBuilder.CreateTable(
        "Teams",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Name = table.Column<string>("longtext", nullable: true)
        },
        constraints: table => { table.PrimaryKey("PK_Teams", x => x.Id); });

      migrationBuilder.CreateTable(
        "Nodes",
        table => new
        {
          IsDeleted = table.Column<bool>("bit", nullable: true),
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Discriminator = table.Column<string>("longtext", nullable: false),
          GameId = table.Column<int>("int", nullable: true),
          Latitude = table.Column<double>("double", nullable: false),
          Longitude = table.Column<double>("double", nullable: false),
          Name = table.Column<string>("longtext", nullable: true),
          NodeId = table.Column<int>("int", nullable: true),
          PictureNode_IsDeleted = table.Column<bool>("bit", nullable: true),
          QuestionNode_IsDeleted = table.Column<bool>("bit", nullable: true),
          Question = table.Column<string>("longtext", nullable: true),
          Delay = table.Column<int>("int", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Nodes", x => x.Id);
          table.ForeignKey(
            "FK_Nodes_Games_GameId",
            x => x.GameId,
            "Games",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_Nodes_Nodes_NodeId",
            x => x.NodeId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateTable(
        "Answers",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          NodeId = table.Column<int>("int", nullable: true),
          QuestionNodeId = table.Column<int>("int", nullable: true),
          Response = table.Column<string>("longtext", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Answers", x => x.Id);
          table.ForeignKey(
            "FK_Answers_Nodes_NodeId",
            x => x.NodeId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_Answers_Nodes_QuestionNodeId",
            x => x.QuestionNodeId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateTable(
        "Players",
        table => new
        {
          Id = table.Column<int>("int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          CurrentNodeId = table.Column<int>("int", nullable: true),
          IsDeleted = table.Column<bool>("bit", nullable: false),
          Name = table.Column<string>("longtext", nullable: true),
          StartTime = table.Column<DateTime>("datetime(6)", nullable: true),
          TeamId = table.Column<int>("int", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Players", x => x.Id);
          table.ForeignKey(
            "FK_Players_Nodes_CurrentNodeId",
            x => x.CurrentNodeId,
            "Nodes",
            "Id",
            onDelete: ReferentialAction.Restrict);
          table.ForeignKey(
            "FK_Players_Teams_TeamId",
            x => x.TeamId,
            "Teams",
            "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateIndex(
        "IX_Answers_NodeId",
        "Answers",
        "NodeId");

      migrationBuilder.CreateIndex(
        "IX_Answers_QuestionNodeId",
        "Answers",
        "QuestionNodeId");

      migrationBuilder.CreateIndex(
        "IX_Nodes_GameId",
        "Nodes",
        "GameId");

      migrationBuilder.CreateIndex(
        "IX_Nodes_NodeId",
        "Nodes",
        "NodeId");

      migrationBuilder.CreateIndex(
        "IX_Players_CurrentNodeId",
        "Players",
        "CurrentNodeId");

      migrationBuilder.CreateIndex(
        "IX_Players_TeamId",
        "Players",
        "TeamId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "Answers");

      migrationBuilder.DropTable(
        "Players");

      migrationBuilder.DropTable(
        "Nodes");

      migrationBuilder.DropTable(
        "Teams");

      migrationBuilder.DropTable(
        "Games");
    }
  }
}
