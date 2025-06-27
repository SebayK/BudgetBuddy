using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddUserShareBudgetRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // usunięcie starej relacji wiele-do-wielu
            migrationBuilder.DropTable(
                name: "ShareBudgetsUser");

            // nowe pola w ShareBudgets
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ShareBudgets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "ShareBudgets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // nowa tabela z relacją wiele-do-wielu + rola
            migrationBuilder.CreateTable(
                name: "UserShareBudgets",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShareBudgetId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShareBudgets", x => new { x.UserId, x.ShareBudgetId });
                    table.ForeignKey(
                        name: "FK_UserShareBudgets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserShareBudgets_ShareBudgets_ShareBudgetId",
                        column: x => x.ShareBudgetId,
                        principalTable: "ShareBudgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShareBudgets_ShareBudgetId",
                table: "UserShareBudgets",
                column: "ShareBudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserShareBudgets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShareBudgets");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "ShareBudgets");

            migrationBuilder.CreateTable(
                name: "ShareBudgetsUser",
                columns: table => new
                {
                    ShareBudgetsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareBudgetsUser", x => new { x.ShareBudgetsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ShareBudgetsUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareBudgetsUser_ShareBudgets_ShareBudgetsId",
                        column: x => x.ShareBudgetsId,
                        principalTable: "ShareBudgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareBudgetsUser_UsersId",
                table: "ShareBudgetsUser",
                column: "UsersId");
        }
    }
}
