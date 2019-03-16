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
                name: "TeamPasscode",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    PasscodeId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPasscode", x => new { x.TeamId, x.PasscodeId });
                    table.ForeignKey(
                        name: "FK_TeamPasscode_Passcodes_PasscodeId",
                        column: x => x.PasscodeId,
                        principalTable: "Passcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPasscode_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPasscode_PasscodeId",
                table: "TeamPasscode",
                column: "PasscodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPasscode");
        }
    }
}
