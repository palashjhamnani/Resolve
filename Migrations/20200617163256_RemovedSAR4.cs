using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class RemovedSAR4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAR4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAR4",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    AbsenceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbsenceReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsenceRequested = table.Column<int>(type: "int", nullable: false),
                    GradYear = table.Column<int>(type: "int", nullable: false),
                    MakeupPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Student = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
        }
    }
}
