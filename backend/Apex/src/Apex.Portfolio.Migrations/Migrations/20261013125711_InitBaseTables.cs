using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Apex.Portfolio.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitBaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinanceInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    Ticker = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Issuer = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialMarket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RefinancingRate = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialMarket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bond",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PaymentFrequencyId = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaturityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentDay = table.Column<int>(type: "int", nullable: false),
                    ParPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: false),
                    IsFloating = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bond", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bond_FinanceInstrument_Id",
                        column: x => x.Id,
                        principalTable: "FinanceInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bond_FinancialAttribute_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "FinancialAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bond_FinancialAttribute_PaymentFrequencyId",
                        column: x => x.PaymentFrequencyId,
                        principalTable: "FinancialAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bond_CurrencyId",
                table: "Bond",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_PaymentFrequencyId",
                table: "Bond",
                column: "PaymentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAttribute_Type_Name",
                table: "FinancialAttribute",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialMarket_Name",
                table: "FinancialMarket",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bond");

            migrationBuilder.DropTable(
                name: "FinancialMarket");

            migrationBuilder.DropTable(
                name: "FinanceInstrument");

            migrationBuilder.DropTable(
                name: "FinancialAttribute");
        }
    }
}
