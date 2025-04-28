using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class addteacherid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSchedules_FacultyMembers_FacultyMemberTeacherId",
                table: "StudentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_StudentSchedules_FacultyMemberTeacherId",
                table: "StudentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_UserId",
                table: "FollowUps");

            migrationBuilder.DropColumn(
                name: "FacultyMemberTeacherId",
                table: "StudentSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "StudentSchedules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedules_TeacherId",
                table: "StudentSchedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_UserId",
                table: "FollowUps",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSchedules_FacultyMembers_TeacherId",
                table: "StudentSchedules",
                column: "TeacherId",
                principalTable: "FacultyMembers",
                principalColumn: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSchedules_FacultyMembers_TeacherId",
                table: "StudentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_StudentSchedules_TeacherId",
                table: "StudentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_UserId",
                table: "FollowUps");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "StudentSchedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacultyMemberTeacherId",
                table: "StudentSchedules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedules_FacultyMemberTeacherId",
                table: "StudentSchedules",
                column: "FacultyMemberTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_UserId",
                table: "FollowUps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSchedules_FacultyMembers_FacultyMemberTeacherId",
                table: "StudentSchedules",
                column: "FacultyMemberTeacherId",
                principalTable: "FacultyMembers",
                principalColumn: "TeacherId");
        }
    }
}
