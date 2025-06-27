using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_Budget_BudgetId",
                table: "Goal");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Budget_BudgetId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBudget_AspNetUsers_UserId",
                table: "UserBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBudget_Budget_BudgetId",
                table: "UserBudget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBudget",
                table: "UserBudget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budget",
                table: "Budget");

            migrationBuilder.RenameTable(
                name: "UserBudget",
                newName: "UserBudgets");

            migrationBuilder.RenameTable(
                name: "Budget",
                newName: "Budgets");

            migrationBuilder.RenameIndex(
                name: "IX_UserBudget_BudgetId",
                table: "UserBudgets",
                newName: "IX_UserBudgets_BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBudgets",
                table: "UserBudgets",
                columns: new[] { "UserId", "BudgetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_Budgets_BudgetId",
                table: "Goal",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Budgets_BudgetId",
                table: "Transaction",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBudgets_AspNetUsers_UserId",
                table: "UserBudgets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBudgets_Budgets_BudgetId",
                table: "UserBudgets",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_Budgets_BudgetId",
                table: "Goal");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Budgets_BudgetId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBudgets_AspNetUsers_UserId",
                table: "UserBudgets");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBudgets_Budgets_BudgetId",
                table: "UserBudgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBudgets",
                table: "UserBudgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "UserBudgets",
                newName: "UserBudget");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "Budget");

            migrationBuilder.RenameIndex(
                name: "IX_UserBudgets_BudgetId",
                table: "UserBudget",
                newName: "IX_UserBudget_BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBudget",
                table: "UserBudget",
                columns: new[] { "UserId", "BudgetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budget",
                table: "Budget",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_Budget_BudgetId",
                table: "Goal",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Budget_BudgetId",
                table: "Transaction",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBudget_AspNetUsers_UserId",
                table: "UserBudget",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBudget_Budget_BudgetId",
                table: "UserBudget",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
