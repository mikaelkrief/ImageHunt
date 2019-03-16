using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageHunt.Migrations
{
  [ExcludeFromCodeCoverage]
  public partial class AddReviewStatusForGameAction : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<DateTime>(
        "DateReviewed",
        "GameActions",
        nullable: false,
        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

      migrationBuilder.AddColumn<bool>(
        "IsReviewed",
        "GameActions",
        nullable: false,
        defaultValue: false);

      migrationBuilder.AddColumn<int>(
        "ReviewerId",
        "GameActions",
        nullable: true);

      migrationBuilder.CreateIndex(
        "IX_GameActions_ReviewerId",
        "GameActions",
        "ReviewerId");

      migrationBuilder.AddForeignKey(
        "FK_GameActions_Admins_ReviewerId",
        "GameActions",
        "ReviewerId",
        "Admins",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        "FK_GameActions_Admins_ReviewerId",
        "GameActions");

      migrationBuilder.DropIndex(
        "IX_GameActions_ReviewerId",
        "GameActions");

      migrationBuilder.DropColumn(
        "DateReviewed",
        "GameActions");

      migrationBuilder.DropColumn(
        "IsReviewed",
        "GameActions");

      migrationBuilder.DropColumn(
        "ReviewerId",
        "GameActions");
    }
  }
}
