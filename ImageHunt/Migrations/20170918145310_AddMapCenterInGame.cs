using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImageHunt.Migrations
{
    public partial class AddMapCenterInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MapCenterLat",
                table: "Games",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MapCenterLng",
                table: "Games",
                type: "double",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapCenterLat",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MapCenterLng",
                table: "Games");
        }
    }
}
