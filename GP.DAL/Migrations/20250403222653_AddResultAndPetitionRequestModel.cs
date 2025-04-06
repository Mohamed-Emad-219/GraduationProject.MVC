using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class AddResultAndPetitionRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetitionRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeanId = table.Column<int>(type: "int", nullable: false),
                    CollegeId = table.Column<int>(type: "int", nullable: false),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfCourses = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<double>(type: "float", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetitionRequests_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetitionRequests_Departments_DeptId",
                        column: x => x.DeptId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetitionRequests_FacultyMembers_DeanId",
                        column: x => x.DeanId,
                        principalTable: "FacultyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetitionCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionRequestId = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitionCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetitionCourses_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetitionCourses_PetitionRequests_PetitionRequestId",
                        column: x => x.PetitionRequestId,
                        principalTable: "PetitionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultPetitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionCourseId = table.Column<int>(type: "int", nullable: false),
                    ErrorInCorrection = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    YearWorkAssessment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracticalExamAssessment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalExamAssessment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGrade = table.Column<bool>(type: "bit", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdvisorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultPetitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultPetitions_Advisors_AdvisorId",
                        column: x => x.AdvisorId,
                        principalTable: "Advisors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResultPetitions_PetitionCourses_PetitionCourseId",
                        column: x => x.PetitionCourseId,
                        principalTable: "PetitionCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetitionCourses_CourseCode",
                table: "PetitionCourses",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionCourses_PetitionRequestId",
                table: "PetitionCourses",
                column: "PetitionRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionRequests_CollegeId",
                table: "PetitionRequests",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionRequests_DeanId",
                table: "PetitionRequests",
                column: "DeanId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitionRequests_DeptId",
                table: "PetitionRequests",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultPetitions_AdvisorId",
                table: "ResultPetitions",
                column: "AdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultPetitions_PetitionCourseId",
                table: "ResultPetitions",
                column: "PetitionCourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultPetitions");

            migrationBuilder.DropTable(
                name: "PetitionCourses");

            migrationBuilder.DropTable(
                name: "PetitionRequests");
        }
    }
}
