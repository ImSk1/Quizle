using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.DB.Migrations
{
    public partial class BadgeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionDate",
                table: "Badges");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcquisitionDate",
                table: "ApplicationUsersBadges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOnProfile",
                table: "ApplicationUsersBadges",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionDate",
                table: "ApplicationUsersBadges");

            migrationBuilder.DropColumn(
                name: "IsOnProfile",
                table: "ApplicationUsersBadges");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcquisitionDate",
                table: "Badges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
