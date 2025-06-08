using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Zamień wartości tekstowe na liczby (przykład: 'Owner' -> 1, 'User' -> 2)
            // migrationBuilder.Sql(
            //     @"UPDATE [UserBudget] SET [Role] = 
            // CASE 
            //     WHEN [Role] = 'Editor' THEN '1'
            //     WHEN [Role] = 'Viewer' THEN '2'
            //     ELSE '0' -- lub inna wartość domyślna
            // END"
            // );
            //
            // migrationBuilder.Sql(@"
            //     DECLARE @constraintName NVARCHAR(128);
            //     SELECT @constraintName = dc.name
            //     FROM [sys].default_constraints dc
            //     INNER JOIN [sys].columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
            //     WHERE dc.parent_object_id = OBJECT_ID(N'[UserBudget]') AND c.name = N'Role';
            //     IF @constraintName IS NOT NULL
            //         EXEC('ALTER TABLE [UserBudget] DROP CONSTRAINT [' + @constraintName + ']');
            //         ");
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "UserBudget",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Transaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextOccurrenceDate",
                table: "Transaction",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurrenceInterval",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Transaction_CategoryId",
            //     table: "Transaction",
            //     column: "CategoryId");
            //
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Transaction_Category_CategoryId",
            //     table: "Transaction",
            //     column: "CategoryId",
            //     principalTable: "Category",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE [UserBudget] SET [Role] = 
            CASE 
                WHEN [Role] = 'Editor' THEN '1'
                WHEN [Role] = 'Viewer' THEN '2'
                ELSE '0'
            END"
            );
            
            migrationBuilder.Sql(
                @"DECLARE @var sysname;
          SELECT @var = [d].[name]
          FROM [sys].[default_constraints] [d]
          INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
          WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserBudget]') AND [c].[name] = N'Role');
          IF @var IS NOT NULL EXEC(N'ALTER TABLE [UserBudget] DROP CONSTRAINT [' + @var + '];');
          ALTER TABLE [UserBudget] ALTER COLUMN [Role] int NOT NULL;"
            );
            
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CategoryId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "NextOccurrenceDate",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transaction");

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
