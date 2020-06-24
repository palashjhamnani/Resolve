using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUser",
                columns: table => new
                {
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUser", x => x.LocalUserID);
                    table.UniqueConstraint("AK_LocalUser_EmailID", x => x.EmailID);
                });

            migrationBuilder.CreateTable(
                name: "EmailPreference",
                columns: table => new
                {
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseCreation = table.Column<bool>(type: "bit", nullable: false),
                    CaseAssignment = table.Column<bool>(type: "bit", nullable: false),
                    CommentCreation = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentCreation = table.Column<bool>(type: "bit", nullable: false),
                    CaseProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailPreference", x => x.LocalUserID);
                    table.ForeignKey(
                        name: "FK_EmailPreference_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalGroup",
                columns: table => new
                {
                    LocalGroupID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalGroup", x => x.LocalGroupID);
                    table.UniqueConstraint("AK_LocalGroup_GroupName", x => x.GroupName);
                    table.ForeignKey(
                        name: "FK_LocalGroup_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseType",
                columns: table => new
                {
                    CaseTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTypeTitle = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalGroupID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GroupNumber = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseType", x => x.CaseTypeID);
                    table.UniqueConstraint("AK_CaseType_CaseTypeTitle", x => x.CaseTypeTitle);
                    table.ForeignKey(
                        name: "FK_CaseType_LocalGroup_LocalGroupID",
                        column: x => x.LocalGroupID,
                        principalTable: "LocalGroup",
                        principalColumn: "LocalGroupID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocalGroupID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.LocalUserID, x.LocalGroupID });
                    table.ForeignKey(
                        name: "FK_UserGroup_LocalGroup_LocalGroupID",
                        column: x => x.LocalGroupID,
                        principalTable: "LocalGroup",
                        principalColumn: "LocalGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Case",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseCID = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "'CASE' + CONVERT([nvarchar](23),[CaseID]+10000000)"),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OnBehalfOf = table.Column<bool>(type: "bit", nullable: false),
                    CaseStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseCreationTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CaseTypeID = table.Column<int>(type: "int", nullable: false),
                    Processed = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Case", x => x.CaseID);
                    table.ForeignKey(
                        name: "FK_Case_CaseType_CaseTypeID",
                        column: x => x.CaseTypeID,
                        principalTable: "CaseType",
                        principalColumn: "CaseTypeID");
                    table.ForeignKey(
                        name: "FK_Case_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID");
                });

            migrationBuilder.CreateTable(
                name: "CaseTypeGroup",
                columns: table => new
                {
                    CaseTypeID = table.Column<int>(type: "int", nullable: false),
                    LocalGroupID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseTypeGroup", x => new { x.CaseTypeID, x.LocalGroupID });
                    table.ForeignKey(
                        name: "FK_CaseTypeGroup_CaseType_CaseTypeID",
                        column: x => x.CaseTypeID,
                        principalTable: "CaseType",
                        principalColumn: "CaseTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseTypeGroup_LocalGroup_LocalGroupID",
                        column: x => x.LocalGroupID,
                        principalTable: "LocalGroup",
                        principalColumn: "LocalGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Approver",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Approved = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approver", x => new { x.CaseID, x.LocalUserID });
                    table.ForeignKey(
                        name: "FK_Approver_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Approver_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseAttachment",
                columns: table => new
                {
                    CaseAttachmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseAttachment", x => x.CaseAttachmentID);
                    table.ForeignKey(
                        name: "FK_CaseAttachment_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseAttachment_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseAudit",
                columns: table => new
                {
                    CaseAuditID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    AuditLog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseAudit", x => x.CaseAuditID);
                    table.ForeignKey(
                        name: "FK_CaseAudit_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseAudit_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseComment",
                columns: table => new
                {
                    CaseCommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseComment", x => x.CaseCommentID);
                    table.ForeignKey(
                        name: "FK_CaseComment_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseComment_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupAssignment",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalGroupID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAssignment", x => new { x.CaseID, x.LocalGroupID });
                    table.ForeignKey(
                        name: "FK_GroupAssignment_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAssignment_LocalGroup_LocalGroupID",
                        column: x => x.LocalGroupID,
                        principalTable: "LocalGroup",
                        principalColumn: "LocalGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "OnBehalf",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    LocalUserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnBehalf", x => new { x.CaseID, x.LocalUserID });
                    table.ForeignKey(
                        name: "FK_OnBehalf_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnBehalf_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleCaseType",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    CaseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleCaseType", x => x.CaseID);
                    table.ForeignKey(
                        name: "FK_SampleCaseType_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SAR4",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    AbsenceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MakeupPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsenceRequested = table.Column<int>(type: "int", nullable: false),
                    Student = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradYear = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsenceReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAR4", x => x.CaseID);
                    table.ForeignKey(
                        name: "FK_SAR4_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Approver_LocalUserID",
                table: "Approver",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Case_CaseTypeID",
                table: "Case",
                column: "CaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Case_LocalUserID",
                table: "Case",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAttachment_CaseID",
                table: "CaseAttachment",
                column: "CaseID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAttachment_LocalUserID",
                table: "CaseAttachment",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAudit_CaseID",
                table: "CaseAudit",
                column: "CaseID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAudit_LocalUserID",
                table: "CaseAudit",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComment_CaseID",
                table: "CaseComment",
                column: "CaseID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComment_LocalUserID",
                table: "CaseComment",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseType_LocalGroupID",
                table: "CaseType",
                column: "LocalGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypeGroup_LocalGroupID",
                table: "CaseTypeGroup",
                column: "LocalGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignment_LocalGroupID",
                table: "GroupAssignment",
                column: "LocalGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_LocalGroup_LocalUserID",
                table: "LocalGroup",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OnBehalf_LocalUserID",
                table: "OnBehalf",
                column: "LocalUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_LocalGroupID",
                table: "UserGroup",
                column: "LocalGroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Approver");

            migrationBuilder.DropTable(
                name: "CaseAttachment");

            migrationBuilder.DropTable(
                name: "CaseAudit");

            migrationBuilder.DropTable(
                name: "CaseComment");

            migrationBuilder.DropTable(
                name: "CaseTypeGroup");

            migrationBuilder.DropTable(
                name: "EmailPreference");

            migrationBuilder.DropTable(
                name: "GroupAssignment");

            migrationBuilder.DropTable(
                name: "HRServiceFaculty");

            migrationBuilder.DropTable(
                name: "HRServiceGradStudent");

            migrationBuilder.DropTable(
                name: "HRServiceStaff");

            migrationBuilder.DropTable(
                name: "OnBehalf");

            migrationBuilder.DropTable(
                name: "SampleCaseType");

            migrationBuilder.DropTable(
                name: "SAR4");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "Case");

            migrationBuilder.DropTable(
                name: "CaseType");

            migrationBuilder.DropTable(
                name: "LocalGroup");

            migrationBuilder.DropTable(
                name: "LocalUser");
        }
    }
}
