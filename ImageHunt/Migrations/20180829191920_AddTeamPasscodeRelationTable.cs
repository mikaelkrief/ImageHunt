using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddTeamPasscodeRelationTable : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        "TeamPasscode",
        table => new
        {
          TeamId = table.Column<int>(nullable: false),
          PasscodeId = table.Column<int>(nullable: false),
          IsDeleted = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_TeamPasscode", x => new {x.TeamId, x.PasscodeId});
          table.ForeignKey(
            "FK_TeamPasscode_Passcodes_PasscodeId",
            x => x.PasscodeId,
            "Passcodes",
            "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            "FK_TeamPasscode_Teams_TeamId",
            x => x.TeamId,
            "Teams",
            "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        "IX_TeamPasscode_PasscodeId",
        "TeamPasscode",
        "PasscodeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        "TeamPasscode");
    }
  }
}
