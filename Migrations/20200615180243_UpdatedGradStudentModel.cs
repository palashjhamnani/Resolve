using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class UpdatedGradStudentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "HRServiceGradStudent");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HRServiceGradStudent");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "HRServiceGradStudent",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "HRServiceGradStudent",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "HRServiceGradStudent");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "HRServiceGradStudent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "HRServiceGradStudent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HRServiceGradStudent",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
