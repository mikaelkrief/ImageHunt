using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddCorrectAnswerInGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_CorrectAnswerId",
                table: "GameActions",
                column: "CorrectAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Answers_CorrectAnswerId",
                table: "GameActions",
                column: "CorrectAnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Answers_CorrectAnswerId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_CorrectAnswerId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswerId",
                table: "GameActions");
        }
    }
}
