using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.DB.Migrations
{
    public partial class QuizPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizPoints",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizPoints",
                table: "AspNetUsers");
        }
    }
}
