using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUser",
                columns: table => new
                {
                    LocalUserID = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUser", x => x.LocalUserID);
                });

            migrationBuilder.CreateTable(
                name: "LocalGroup",
                columns: table => new
                {
                    LocalGroupID = table.Column<string>(nullable: false),
                    GroupName = table.Column<string>(nullable: false),
                    GroupDescription = table.Column<string>(nullable: true),
                    LocalUserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalGroup", x => x.LocalGroupID);
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
                    CaseTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTypeTitle = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    LocalGroupID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseType", x => x.CaseTypeID);
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
                    LocalUserID = table.Column<string>(nullable: false),
                    LocalGroupID = table.Column<string>(nullable: false)
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
                    CaseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalUserID = table.Column<string>(nullable: true),
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
                        name: "FK_Case_LocalUser_LocalUserID",
                        column: x => x.LocalUserID,
                        principalTable: "LocalUser",
                        principalColumn: "LocalUserID");
                });

            migrationBuilder.CreateTable(
                name: "Approver",
                columns: table => new
                {
                    CaseID = table.Column<int>(nullable: false),
                    LocalUserID = table.Column<string>(nullable: false),
                    Approved = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
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
                    CaseAttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseID = table.Column<int>(nullable: false),
                    LocalUserID = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    AttachmentTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
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
                    CaseAuditID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    AuditLog = table.Column<string>(nullable: false),
                    CaseID = table.Column<int>(nullable: false),
                    LocalUserID = table.Column<string>(nullable: true)
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
                    CaseCommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(nullable: false),
                    CommentTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CaseID = table.Column<int>(nullable: false),
                    LocalUserID = table.Column<string>(nullable: true)
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
                    CaseID = table.Column<int>(nullable: false),
                    LocalGroupID = table.Column<string>(nullable: false)
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
                name: "OnBehalf",
                columns: table => new
                {
                    CaseID = table.Column<int>(nullable: false),
                    LocalUserID = table.Column<string>(nullable: false)
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
                name: "IX_SampleCaseType_CaseID1",
                table: "SampleCaseType",
                column: "CaseID1");

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
                name: "GroupAssignment");

            migrationBuilder.DropTable(
                name: "OnBehalf");

            migrationBuilder.DropTable(
                name: "SampleCaseType");

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
