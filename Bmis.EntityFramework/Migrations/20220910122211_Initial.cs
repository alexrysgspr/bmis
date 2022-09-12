using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bmis.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bmis");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "bmis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Purok = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blotters",
                schema: "bmis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Complainant = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Respondent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BlotterType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blotters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Residents",
                schema: "bmis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CivilStatus = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    VoterStatus = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsPwd = table.Column<bool>(type: "bit", nullable: false),
                    Disability = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Residents_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "bmis",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Residents_AddressId",
                schema: "bmis",
                table: "Residents",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blotters",
                schema: "bmis");

            migrationBuilder.DropTable(
                name: "Residents",
                schema: "bmis");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "bmis");
        }
    }
}
