using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class AllowNullEnums2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceFaculty_FacAllowanceChange_Enum_Constraint",
                table: "HRServiceFaculty");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceFaculty_SupOrg_Enum_Constraint",
                table: "HRServiceFaculty");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceFaculty_TerminationReason_Enum_Constraint",
                table: "HRServiceFaculty");

            migrationBuilder.AlterColumn<string>(
                name: "BudgetNumbers",
                table: "HRServiceStaff",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TerminationReason",
                table: "HRServiceFaculty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SupOrg",
                table: "HRServiceFaculty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Offboarding",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "LeaveWA",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "FacAllowanceChange",
                table: "HRServiceFaculty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "ClosePosition",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceFaculty_FacAllowanceChange_Enum_Constraint",
                table: "HRServiceFaculty",
                sql: "[FacAllowanceChange] IN(0, 1, 2, 3, 4)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceFaculty_SupOrg_Enum_Constraint",
                table: "HRServiceFaculty",
                sql: "[SupOrg] IN(0, 1, 2, 3)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceFaculty_TerminationReason_Enum_Constraint",
                table: "HRServiceFaculty",
                sql: "[TerminationReason] IN(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22)");

            migrationBuilder.AlterColumn<string>(
                name: "BudgetNumbers",
                table: "HRServiceStaff",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TerminationReason",
                table: "HRServiceFaculty",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SupOrg",
                table: "HRServiceFaculty",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Offboarding",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LeaveWA",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FacAllowanceChange",
                table: "HRServiceFaculty",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ClosePosition",
                table: "HRServiceFaculty",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
