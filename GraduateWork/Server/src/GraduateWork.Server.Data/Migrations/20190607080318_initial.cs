using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GraduateWork.Server.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entrants",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    firs_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    birthday = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entrants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "specialities",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    faculty = table.Column<string>(nullable: true),
                    subject_scores = table.Column<string>(nullable: true),
                    count_of_places = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "certificate_of_secondary_education",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    serial_number = table.Column<string>(nullable: true),
                    average_mark = table.Column<float>(nullable: false),
                    full_name_of_the_educational_institution = table.Column<string>(nullable: true),
                    year_of_issue = table.Column<DateTime>(nullable: false),
                    entrant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_of_secondary_education", x => x.id);
                    table.ForeignKey(
                        name: "FK_certificate_of_secondary_education_entrants_entrant_id",
                        column: x => x.entrant_id,
                        principalTable: "entrants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificates_of_testing",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    serial_number = table.Column<string>(nullable: true),
                    year_of_issue = table.Column<DateTime>(nullable: false),
                    first_subject = table.Column<string>(nullable: true),
                    second_subject = table.Column<string>(nullable: true),
                    third_subject = table.Column<string>(nullable: true),
                    fourth_subject = table.Column<string>(nullable: true),
                    first_mark = table.Column<float>(nullable: false),
                    second_mark = table.Column<float>(nullable: false),
                    third_mark = table.Column<float>(nullable: false),
                    fourth_mark = table.Column<float>(nullable: false),
                    entrant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificates_of_testing", x => x.id);
                    table.ForeignKey(
                        name: "FK_certificates_of_testing_entrants_entrant_id",
                        column: x => x.entrant_id,
                        principalTable: "entrants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    birthday = table.Column<DateTimeOffset>(nullable: false),
                    entrant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_entrants_entrant_id",
                        column: x => x.entrant_id,
                        principalTable: "entrants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "universities",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    full_name = table.Column<string>(nullable: true),
                    level_of_accreditation = table.Column<string>(nullable: true),
                    ownership = table.Column<string>(nullable: true),
                    chief = table.Column<string>(nullable: true),
                    subordination = table.Column<string>(nullable: true),
                    post_index = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    site = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    region_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_universities", x => x.id);
                    table.ForeignKey(
                        name: "FK_universities_Regions_region_id",
                        column: x => x.region_id,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "statements",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    total_score = table.Column<float>(nullable: false),
                    extra_score = table.Column<float>(nullable: false),
                    priority = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    entrant_id = table.Column<Guid>(nullable: false),
                    speciality_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statements", x => x.id);
                    table.ForeignKey(
                        name: "FK_statements_entrants_entrant_id",
                        column: x => x.entrant_id,
                        principalTable: "entrants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_statements_specialities_speciality_id",
                        column: x => x.speciality_id,
                        principalTable: "specialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "university_specialities",
                columns: table => new
                {
                    university_id = table.Column<Guid>(nullable: false),
                    specialty_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_university_specialities", x => new { x.specialty_id, x.university_id });
                    table.ForeignKey(
                        name: "FK_university_specialities_specialities_specialty_id",
                        column: x => x.specialty_id,
                        principalTable: "specialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_university_specialities_universities_university_id",
                        column: x => x.university_id,
                        principalTable: "universities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_certificate_of_secondary_education_entrant_id",
                table: "certificate_of_secondary_education",
                column: "entrant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_certificates_of_testing_entrant_id",
                table: "certificates_of_testing",
                column: "entrant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_statements_entrant_id",
                table: "statements",
                column: "entrant_id");

            migrationBuilder.CreateIndex(
                name: "IX_statements_speciality_id",
                table: "statements",
                column: "speciality_id");

            migrationBuilder.CreateIndex(
                name: "IX_universities_region_id",
                table: "universities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_university_specialities_university_id",
                table: "university_specialities",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_entrant_id",
                table: "users",
                column: "entrant_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "certificate_of_secondary_education");

            migrationBuilder.DropTable(
                name: "certificates_of_testing");

            migrationBuilder.DropTable(
                name: "statements");

            migrationBuilder.DropTable(
                name: "university_specialities");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "specialities");

            migrationBuilder.DropTable(
                name: "universities");

            migrationBuilder.DropTable(
                name: "entrants");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
