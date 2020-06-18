using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRServiceFaculty",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacRequestType = table.Column<int>(type: "int", nullable: false),
                    SupOrg = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<int>(type: "int", nullable: false),
                    TerminationReason = table.Column<int>(type: "int", nullable: true),
                    FacAllowanceChange = table.Column<int>(type: "int", nullable: true),
                    EmployeeEID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentFTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedFTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offboarding = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ClosePosition = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LeaveWA = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRServiceFaculty", x => x.CaseID);
                    table.CheckConstraint("CK_HRServiceFaculty_Department_Enum_Constraint", "[Department] IN(0, 1, 2, 3, 4, 5, 6, 7)");
                    table.CheckConstraint("CK_HRServiceFaculty_FacRequestType_Enum_Constraint", "[FacRequestType] IN(0, 1, 2, 3, 4, 5, 6, 7, 8)");
                    table.ForeignKey(
                        name: "FK_HRServiceFaculty_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRServiceGradStudent",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradRequestType = table.Column<int>(type: "int", nullable: false),
                    GradJobProfile = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StepStipendAllowance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRServiceGradStudent", x => x.CaseID);
                    table.CheckConstraint("CK_HRServiceGradStudent_Department_Enum_Constraint", "[Department] IN(0, 1, 2, 3, 4, 5, 6, 7)");
                    table.CheckConstraint("CK_HRServiceGradStudent_GradJobProfile_Enum_Constraint", "[GradJobProfile] IN(0, 1, 2, 3)");
                    table.CheckConstraint("CK_HRServiceGradStudent_GradRequestType_Enum_Constraint", "[GradRequestType] IN(0, 1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_HRServiceGradStudent_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRServiceStaff",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    WorkerType = table.Column<int>(type: "int", nullable: false),
                    BasePayChange = table.Column<int>(type: "int", nullable: true),
                    AllowanceChange = table.Column<int>(type: "int", nullable: true),
                    TerminationReason = table.Column<int>(type: "int", nullable: true),
                    SupOrg = table.Column<int>(type: "int", nullable: true),
                    EmployeeEID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Offboarding = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ClosePosition = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LeaveWA = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRServiceStaff", x => x.CaseID);
                    table.CheckConstraint("CK_HRServiceStaff_RequestType_Enum_Constraint", "[RequestType] IN(0, 1, 2, 3, 4, 5, 6, 7)");
                    table.CheckConstraint("CK_HRServiceStaff_WorkerType_Enum_Constraint", "[WorkerType] IN(0, 1, 2)");
                    table.ForeignKey(
                        name: "FK_HRServiceStaff_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRServiceFaculty");

            migrationBuilder.DropTable(
                name: "HRServiceGradStudent");

            migrationBuilder.DropTable(
                name: "HRServiceStaff");
        }
    }
}
