using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHuntCore.Migrations
{
    public partial class AdminTokenExpirationDateCanBeNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationTokenDate",
                table: "Admins",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationTokenDate",
                table: "Admins",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
