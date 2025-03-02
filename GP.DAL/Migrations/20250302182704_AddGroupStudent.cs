using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class AddGroupStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Students");
        }
    }
}
