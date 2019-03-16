using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class AddGameActionIndex : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@"CREATE INDEX IX_GAME_TEAM on GameActions (GameId, TeamId, NodeId)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@"DROP INDEX IX_GAME_TEAM on GameActions");
    }
  }
}
