using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizle.DB.Migrations
{
    public partial class ChangedUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAnsweredCurrentQuestion",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAnsweredCurrentQuestion",
                table: "AspNetUsers");
        }
    }
}
