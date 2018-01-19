using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GraduateWorkApi.Migrations
{
    public partial class AddnewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BDay = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specialtys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalFactor = table.Column<float>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CountOfStatePlaces = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialtys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universitys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    LevelOfAccreditation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateOfSecondaryEducations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AverageMark = table.Column<float>(nullable: false),
                    EntrantId = table.Column<int>(nullable: true),
                    FullNameOfTheEducationalInstitution = table.Column<string>(nullable: true),
                    SeriaNumber = table.Column<string>(nullable: true),
                    YearOfIssue = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateOfSecondaryEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateOfSecondaryEducations_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificateOfTestings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntrantId = table.Column<int>(nullable: true),
                    FirstMark = table.Column<float>(nullable: false),
                    FirstSubject = table.Column<string>(nullable: true),
                    FourthMark = table.Column<float>(nullable: false),
                    FourthSubject = table.Column<string>(nullable: true),
                    SecondMark = table.Column<float>(nullable: false),
                    SecondSubject = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    ThirdMark = table.Column<float>(nullable: false),
                    ThirdSubject = table.Column<string>(nullable: true),
                    YearOfIssue = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateOfTestings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateOfTestings_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntrantId = table.Column<int>(nullable: false),
                    ExtraScore = table.Column<float>(nullable: false),
                    TotalScore = table.Column<float>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statements_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statements_Universitys_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversitySpeciality",
                columns: table => new
                {
                    SpecialtyId = table.Column<int>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversitySpeciality", x => new { x.SpecialtyId, x.UniversityId });
                    table.ForeignKey(
                        name: "FK_UniversitySpeciality_Specialtys_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialtys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversitySpeciality_Universitys_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateOfSecondaryEducations_EntrantId",
                table: "CertificateOfSecondaryEducations",
                column: "EntrantId",
                unique: true,
                filter: "[EntrantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateOfTestings_EntrantId",
                table: "CertificateOfTestings",
                column: "EntrantId",
                unique: true,
                filter: "[EntrantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_UserId",
                table: "Entrants",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_EntrantId",
                table: "Statements",
                column: "EntrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_UniversityId",
                table: "Statements",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversitySpeciality_UniversityId",
                table: "UniversitySpeciality",
                column: "UniversityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateOfSecondaryEducations");

            migrationBuilder.DropTable(
                name: "CertificateOfTestings");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "UniversitySpeciality");

            migrationBuilder.DropTable(
                name: "Entrants");

            migrationBuilder.DropTable(
                name: "Specialtys");

            migrationBuilder.DropTable(
                name: "Universitys");
        }
    }
}
