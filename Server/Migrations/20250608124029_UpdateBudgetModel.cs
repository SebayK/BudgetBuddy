using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Migrations
{
    public partial class UpdateBudgetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "UserBudget",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // ❌ USUWAMY powielone kolumny – już istnieją w bazie
            // migrationBuilder.AddColumn<string>("Description", ...)
            // migrationBuilder.AddColumn<bool>("IsRecurring", ...)
            // migrationBuilder.AddColumn<DateTime>("NextOccurrenceDate", ...)
            // migrationBuilder.AddColumn<string>("RecurrenceInterval", ...)
            // migrationBuilder.AddColumn<string>("Type", ...)
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ✅ Nie próbujemy usuwać kolumn, których nie dodaliśmy tutaj
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "UserBudget",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}