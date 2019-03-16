using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddbotUserToAdmin : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData("Admins",
        new[] {"Email", "ExpirationTokenDate", "Name", "IsDeleted", "Token", "Role"},
        new object[]
          {"imagehuntbot@bot.com", DateTime.Today.AddYears(1000), "ImageHuntBot", false, "ImageHuntBotToken", 4});
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}
