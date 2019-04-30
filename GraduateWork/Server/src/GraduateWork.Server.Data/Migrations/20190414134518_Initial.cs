﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduateWork.Server.Data.Migrations
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Initial : Migration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "specialities",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    additional_factory = table.Column<float>(nullable: false),
                    count_of_state_places = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "universities",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    full_name = table.Column<string>(nullable: true),
                    level_of_accreditation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_universities", x => x.id);
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
                    birthday = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "entrants",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    firs_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    birthday = table.Column<DateTimeOffset>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entrants", x => x.id);
                    table.ForeignKey(
                        name: "FK_entrants_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "statements",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    total_score = table.Column<float>(nullable: false),
                    extra_score = table.Column<float>(nullable: false),
                    entrant_id = table.Column<Guid>(nullable: false),
                    university_id = table.Column<Guid>(nullable: false)
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
                        name: "FK_statements_universities_university_id",
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
                name: "IX_entrants_user_id",
                table: "entrants",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_statements_entrant_id",
                table: "statements",
                column: "entrant_id");

            migrationBuilder.CreateIndex(
                name: "IX_statements_university_id",
                table: "statements",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "IX_university_specialities_university_id",
                table: "university_specialities",
                column: "university_id");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="migrationBuilder"></param>
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
                name: "entrants");

            migrationBuilder.DropTable(
                name: "specialities");

            migrationBuilder.DropTable(
                name: "universities");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
