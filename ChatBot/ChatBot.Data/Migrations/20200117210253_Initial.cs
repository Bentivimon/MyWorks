using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "viber_users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    viber_id = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    avatar = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    is_subscribed = table.Column<bool>(nullable: false),
                    created_timestamp = table.Column<long>(nullable: false),
                    updated_timestamp = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viber_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "viber_user_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    message = table.Column<string>(nullable: true),
                    message_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viber_user_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_viber_user_messages_viber_users_user_id",
                        column: x => x.user_id,
                        principalTable: "viber_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_viber_user_messages_user_id",
                table: "viber_user_messages",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "viber_user_messages");

            migrationBuilder.DropTable(
                name: "viber_users");
        }
    }
}
