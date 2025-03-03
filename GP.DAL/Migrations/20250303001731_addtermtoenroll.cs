using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class addtermtoenroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "Semester",
                table: "Enrollments",
                newName: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_TermId",
                table: "Enrollments",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Terms_TermId",
                table: "Enrollments",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Terms_TermId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_TermId",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "TermId",
                table: "Enrollments",
                newName: "Semester");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
