using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Portfolio.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenameBondIsFloatingToIsRefinancingLinked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFloating",
                table: "Bond",
                newName: "IsRefinancingLinked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRefinancingLinked",
                table: "Bond",
                newName: "IsFloating");
        }
    }
}
