using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.DB.Migrations
{
    [ExcludeFromCodeCoverage]

    public partial class LeaderboardChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizPoints",
                table: "AspNetUsers",
                newName: "CurrentQuizPoints");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcquisitionDate",
                table: "Badges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AllTimeQuizPoints",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionDate",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "AllTimeQuizPoints",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CurrentQuizPoints",
                table: "AspNetUsers",
                newName: "QuizPoints");
        }
    }
}
