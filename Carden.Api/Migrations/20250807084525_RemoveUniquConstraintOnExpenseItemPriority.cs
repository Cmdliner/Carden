using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carden.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniquConstraintOnExpenseItemPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_UserId_Priority",
                table: "ExpenseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_UserId",
                table: "ExpenseItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_UserId",
                table: "ExpenseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_UserId_Priority",
                table: "ExpenseItems",
                columns: new[] { "UserId", "Priority" },
                unique: true);
        }
    }
}
