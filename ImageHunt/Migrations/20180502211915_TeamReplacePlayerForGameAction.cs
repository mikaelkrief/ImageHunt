using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImageHunt.Migrations
{
    public partial class TeamReplacePlayerForGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Players_PlayerId",
                table: "GameActions");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "GameActions",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_GameActions_PlayerId",
                table: "GameActions",
                newName: "IX_GameActions_TeamId");

            migrationBuilder.AddColumn<int>(
                name: "CurrentNodeId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CurrentNodeId",
                table: "Teams",
                column: "CurrentNodeId");

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

            migrationBuilder.DropColumn(
                name: "CurrentNodeId",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "GameActions",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_GameActions_TeamId",
                table: "GameActions",
                newName: "IX_GameActions_PlayerId");

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
