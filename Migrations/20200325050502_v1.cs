using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "CaseType",
                columns: table => new
                {
                    CaseTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTypeTitle = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseType", x => x.CaseTypeID);
                    table.ForeignKey(
                        name: "FK_CaseType_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Case",
                columns: table => new
                {
                    CaseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    OnBehalfOf = table.Column<int>(nullable: false),
                    CaseStatus = table.Column<string>(nullable: true),
                    CaseCreationTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CaseTypeID = table.Column<int>(nullable: false)
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
                        name: "FK_Case_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Approver",
                columns: table => new
                {
                    CaseID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Approved = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approver", x => new { x.CaseID, x.UserID });
                    table.ForeignKey(
                        name: "FK_Approver_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Approver_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseAudit",
                columns: table => new
                {
                    CaseAuditID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    AuditLog = table.Column<string>(nullable: false),
                    CaseID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
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
                        name: "FK_CaseAudit_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseComment",
                columns: table => new
                {
                    CaseCommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(nullable: false),
                    CommentTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CaseID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
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
                        name: "FK_CaseComment_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleCaseType",
                columns: table => new
                {
                    CaseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseID1 = table.Column<int>(nullable: false),
                    CaseDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleCaseType", x => x.CaseID);
                    table.ForeignKey(
                        name: "FK_SampleCaseType_Case_CaseID1",
                        column: x => x.CaseID1,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Approver_UserID",
                table: "Approver",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Case_CaseTypeID",
                table: "Case",
                column: "CaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Case_UserID",
                table: "Case",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAudit_CaseID",
                table: "CaseAudit",
                column: "CaseID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAudit_UserID",
                table: "CaseAudit",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComment_CaseID",
                table: "CaseComment",
                column: "CaseID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComment_UserID",
                table: "CaseComment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CaseType_UserID",
                table: "CaseType",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SampleCaseType_CaseID1",
                table: "SampleCaseType",
                column: "CaseID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Approver");

            migrationBuilder.DropTable(
                name: "CaseAudit");

            migrationBuilder.DropTable(
                name: "CaseComment");

            migrationBuilder.DropTable(
                name: "SampleCaseType");

            migrationBuilder.DropTable(
                name: "Case");

            migrationBuilder.DropTable(
                name: "CaseType");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
