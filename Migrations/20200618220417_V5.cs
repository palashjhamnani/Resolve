using Microsoft.EntityFrameworkCore.Migrations;

namespace Resolve.Migrations
{
    public partial class V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample2");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_SampleCaseType_RequestType_Enum_Constraint",
                table: "SampleCaseType",
                sql: "[RequestType] IN(0, 1, 2, 3, 4, 5, 6, 7, 8)");

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "SampleCaseType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SampleCaseType_RequestType_Enum_Constraint",
                table: "SampleCaseType");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "SampleCaseType");

            migrationBuilder.CreateTable(
                name: "Sample2",
                columns: table => new
                {
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    SampleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample2", x => x.CaseID);
                    table.ForeignKey(
                        name: "FK_Sample2_Case_CaseID",
                        column: x => x.CaseID,
                        principalTable: "Case",
                        principalColumn: "CaseID",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
