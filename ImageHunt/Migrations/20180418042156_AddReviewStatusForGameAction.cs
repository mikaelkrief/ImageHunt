using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]

  public partial class AddReviewStatusForGameAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReviewed",
                table: "GameActions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "GameActions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReviewerId",
                table: "GameActions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_ReviewerId",
                table: "GameActions",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Admins_ReviewerId",
                table: "GameActions",
                column: "ReviewerId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Admins_ReviewerId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_ReviewerId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "DateReviewed",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "GameActions");
        }
    }
}
