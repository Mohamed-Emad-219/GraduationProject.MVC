using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class FollowUpModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUpSchedules");

            migrationBuilder.CreateTable(
                name: "FollowUpEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FollowUpId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpEntries_FollowUps_FollowUpId",
                        column: x => x.FollowUpId,
                        principalTable: "FollowUps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowUpEntries_InstructorSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "InstructorSchedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpEntries_FollowUpId",
                table: "FollowUpEntries",
                column: "FollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpEntries_ScheduleId",
                table: "FollowUpEntries",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUpEntries");

            migrationBuilder.CreateTable(
                name: "FollowUpSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssistantId = table.Column<int>(type: "int", nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowUpId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAttendant = table.Column<bool>(type: "bit", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    TimeBegin = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpSchedules_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowUpSchedules_FacultyMembers_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "FacultyMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowUpSchedules_FacultyMembers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "FacultyMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowUpSchedules_FollowUps_FollowUpId",
                        column: x => x.FollowUpId,
                        principalTable: "FollowUps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowUpSchedules_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSchedules_AssistantId",
                table: "FollowUpSchedules",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSchedules_CourseCode",
                table: "FollowUpSchedules",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSchedules_FollowUpId",
                table: "FollowUpSchedules",
                column: "FollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSchedules_InstructorId",
                table: "FollowUpSchedules",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpSchedules_PlaceId",
                table: "FollowUpSchedules",
                column: "PlaceId");
        }
    }
}
