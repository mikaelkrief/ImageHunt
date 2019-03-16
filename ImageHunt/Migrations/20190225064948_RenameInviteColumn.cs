using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class RenameInviteColumn : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
        "InviteUri",
        "Teams",
        "ChatInviteUrl");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
        "ChatInviteUrl",
        "Teams",
        "InviteUri");
    }
  }
}
