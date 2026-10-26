using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Apex.Portfolio.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenameFinancailAttributeTableToInstrumentMetadataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bond_FinancialAttribute_CurrencyId",
                table: "Bond");

            migrationBuilder.DropForeignKey(
                name: "FK_Bond_FinancialAttribute_PaymentFrequencyId",
                table: "Bond");

            migrationBuilder.DropTable(
                name: "FinancialAttribute");

            migrationBuilder.CreateTable(
                name: "InstrumentMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentMetadata_Type_Name",
                table: "InstrumentMetadata",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bond_InstrumentMetadata_CurrencyId",
                table: "Bond",
                column: "CurrencyId",
                principalTable: "InstrumentMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bond_InstrumentMetadata_PaymentFrequencyId",
                table: "Bond",
                column: "PaymentFrequencyId",
                principalTable: "InstrumentMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bond_InstrumentMetadata_CurrencyId",
                table: "Bond");

            migrationBuilder.DropForeignKey(
                name: "FK_Bond_InstrumentMetadata_PaymentFrequencyId",
                table: "Bond");

            migrationBuilder.DropTable(
                name: "InstrumentMetadata");

            migrationBuilder.CreateTable(
                name: "FinancialAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAttribute", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAttribute_Type_Name",
                table: "FinancialAttribute",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bond_FinancialAttribute_CurrencyId",
                table: "Bond",
                column: "CurrencyId",
                principalTable: "FinancialAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bond_FinancialAttribute_PaymentFrequencyId",
                table: "Bond",
                column: "PaymentFrequencyId",
                principalTable: "FinancialAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
