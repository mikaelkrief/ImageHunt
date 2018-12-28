using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class RemovePlayerFromGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Players_PlayerId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_PlayerId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "GameActions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_PlayerId",
                table: "GameActions",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Players_PlayerId",
                table: "GameActions",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
