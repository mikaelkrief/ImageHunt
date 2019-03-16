using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddTeamInGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentNodeId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CurrentNodeId",
                table: "Teams",
                column: "CurrentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_TeamId",
                table: "GameActions",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Teams_TeamId",
                table: "GameActions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Nodes_CurrentNodeId",
                table: "Teams",
                column: "CurrentNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Teams_TeamId",
                table: "GameActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Nodes_CurrentNodeId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CurrentNodeId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_TeamId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "CurrentNodeId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "GameActions");
        }
    }
}
