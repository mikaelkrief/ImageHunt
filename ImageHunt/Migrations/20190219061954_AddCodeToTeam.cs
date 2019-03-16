using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  public partial class AddCodeToTeam : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        "Code",
        "Teams",
        nullable: true);

      migrationBuilder.AlterColumn<string>(
        "UserName",
        "AspNetUsers",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedUserName",
        "AspNetUsers",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedEmail",
        "AspNetUsers",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "Email",
        "AspNetUsers",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedName",
        "AspNetRoles",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "Name",
        "AspNetRoles",
        unicode: false,
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldMaxLength: 256,
        oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        "Code",
        "Teams");

      migrationBuilder.AlterColumn<string>(
        "UserName",
        "AspNetUsers",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedUserName",
        "AspNetUsers",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedEmail",
        "AspNetUsers",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "Email",
        "AspNetUsers",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "NormalizedName",
        "AspNetRoles",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);

      migrationBuilder.AlterColumn<string>(
        "Name",
        "AspNetRoles",
        maxLength: 256,
        nullable: true,
        oldClrType: typeof(string),
        oldUnicode: false,
        oldMaxLength: 256,
        oldNullable: true);
    }
  }
}
