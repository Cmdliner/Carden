using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carden.Api.Migrations
{
    /// <inheritdoc />
    public partial class IndexPriorityExpenseItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_Priority",
                table: "ExpenseItems",
                column: "Priority",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_Priority",
                table: "ExpenseItems");
        }
    }
}
