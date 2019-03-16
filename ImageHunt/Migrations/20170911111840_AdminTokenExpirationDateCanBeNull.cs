using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AdminTokenExpirationDateCanBeNull : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<DateTime>(
        "ExpirationTokenDate",
        "Admins",
        "datetime(6)",
        nullable: true,
        oldClrType: typeof(DateTime));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<DateTime>(
        "ExpirationTokenDate",
        "Admins",
        nullable: false,
        oldClrType: typeof(DateTime),
        oldType: "datetime(6)",
        oldNullable: true);
    }
  }
}
