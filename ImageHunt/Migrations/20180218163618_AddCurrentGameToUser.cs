using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddCurrentGameToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentGameId",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentGameId",
                table: "Players",
                column: "CurrentGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_CurrentGameId",
                table: "Players",
                column: "CurrentGameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_CurrentGameId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_CurrentGameId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentGameId",
                table: "Players");
        }
    }
}
