using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Portfolio.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenameBondOperationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "BondOperations",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "BondOperations",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "BondOperations",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "BondOperations",
                newName: "Quantity");
        }
    }
}
