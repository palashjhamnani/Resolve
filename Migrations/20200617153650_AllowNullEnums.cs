using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class AllowNullEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Sample2_WorkerType_Enum_Constraint",
                table: "Sample2");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceStaff_AllowanceChange_Enum_Constraint",
                table: "HRServiceStaff");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceStaff_BasePayChange_Enum_Constraint",
                table: "HRServiceStaff");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceStaff_SupOrg_Enum_Constraint",
                table: "HRServiceStaff");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HRServiceStaff_TerminationReason_Enum_Constraint",
                table: "HRServiceStaff");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerType",
                table: "Sample2",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TerminationReason",
                table: "HRServiceStaff",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SupOrg",
                table: "HRServiceStaff",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Offboarding",
                table: "HRServiceStaff",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "LeaveWA",
                table: "HRServiceStaff",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ClosePosition",
                table: "HRServiceStaff",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "BasePayChange",
                table: "HRServiceStaff",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AllowanceChange",
                table: "HRServiceStaff",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "CK_Sample2_WorkerType_Enum_Constraint",
                table: "Sample2",
                sql: "[WorkerType] IN(0, 1, 2)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceStaff_AllowanceChange_Enum_Constraint",
                table: "HRServiceStaff",
                sql: "[AllowanceChange] IN(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceStaff_BasePayChange_Enum_Constraint",
                table: "HRServiceStaff",
                sql: "[BasePayChange] IN(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceStaff_SupOrg_Enum_Constraint",
                table: "HRServiceStaff",
                sql: "[SupOrg] IN(0, 1, 2, 3)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_HRServiceStaff_TerminationReason_Enum_Constraint",
                table: "HRServiceStaff",
                sql: "[TerminationReason] IN(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22)");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerType",
                table: "Sample2",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TerminationReason",
                table: "HRServiceStaff",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SupOrg",
                table: "HRServiceStaff",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Offboarding",
                table: "HRServiceStaff",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LeaveWA",
                table: "HRServiceStaff",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ClosePosition",
                table: "HRServiceStaff",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BasePayChange",
                table: "HRServiceStaff",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AllowanceChange",
                table: "HRServiceStaff",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
