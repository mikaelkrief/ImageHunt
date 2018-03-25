using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImageHunt.Migrations
{
    public partial class AddIsValidatedToGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "GameActions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NodeId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_NodeId",
                table: "GameActions",
                column: "NodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Nodes_NodeId",
                table: "GameActions",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Nodes_NodeId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_NodeId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "NodeId",
                table: "GameActions");
        }
    }
}
