using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Portfolio.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class FinanceInstrumentUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FinanceInstrument",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceInstrument_UserId",
                table: "FinanceInstrument",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinanceInstrument_UserId",
                table: "FinanceInstrument");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FinanceInstrument");
        }
    }
}
