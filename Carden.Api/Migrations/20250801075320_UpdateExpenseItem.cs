using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carden.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExpenseItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExpenseItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExpenseItems");
        }
    }
}
