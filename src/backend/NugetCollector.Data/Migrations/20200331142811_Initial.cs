using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NugetCollector.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => new { x.Name, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "ProjectReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    ReferenceName = table.Column<string>(nullable: true),
                    ReferenceVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReference_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReference_References_ReferenceName_ReferenceVersion",
                        columns: x => new { x.ReferenceName, x.ReferenceVersion },
                        principalTable: "References",
                        principalColumns: new[] { "Name", "Version" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReference_ProjectId",
                table: "ProjectReference",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReference_ReferenceName_ReferenceVersion",
                table: "ProjectReference",
                columns: new[] { "ReferenceName", "ReferenceVersion" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectReference");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "References");
        }
    }
}
