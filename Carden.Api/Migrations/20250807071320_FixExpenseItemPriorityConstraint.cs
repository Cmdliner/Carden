using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carden.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixExpenseItemPriorityConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_Priority",
                table: "ExpenseItems");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_UserId",
                table: "ExpenseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_UserId_Priority",
                table: "ExpenseItems",
                columns: new[] { "UserId", "Priority" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_UserId_Priority",
                table: "ExpenseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_Priority",
                table: "ExpenseItems",
                column: "Priority",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_UserId",
                table: "ExpenseItems",
                column: "UserId");
        }
    }
}
