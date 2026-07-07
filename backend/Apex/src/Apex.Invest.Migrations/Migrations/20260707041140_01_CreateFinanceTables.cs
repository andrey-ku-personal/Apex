using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Apex.Invest.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class _01_CreateFinanceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancePlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancePlatform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinanceInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    Ticker = table.Column<string>(type: "character varying(64)", maxLength: 512, nullable: false),
                    Issuer = table.Column<string>(type: "character varying(256)", maxLength: 512, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceInstrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceInstrument_FinancePlatform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "FinancePlatform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bond",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Frequence = table.Column<int>(type: "int", nullable: false),
                    NominalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MarketPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BrokerCommission = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FirstPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Deposit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Frequence = table.Column<int>(type: "int", nullable: false),
                    IsReplenishable = table.Column<bool>(type: "boolean", nullable: false),
                    FirstPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsTaxFree = table.Column<bool>(type: "boolean", nullable: false),
                    Capitalization = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposit_FinanceInstrument_Id",
                        column: x => x.Id,
                        principalTable: "FinanceInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    MarketPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BrokerCommission = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DividendRate = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Share_FinanceInstrument_Id",
                        column: x => x.Id,
                        principalTable: "FinanceInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepositId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositOperation_Deposit_DepositId",
                        column: x => x.DepositId,
                        principalTable: "Deposit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositOperation_DepositId",
                table: "DepositOperation",
                column: "DepositId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceInstrument_PlatformId",
                table: "FinanceInstrument",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancePlatform_Name",
                table: "FinancePlatform",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bond");

            migrationBuilder.DropTable(
                name: "DepositOperation");

            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.DropTable(
                name: "Deposit");

            migrationBuilder.DropTable(
                name: "FinanceInstrument");

            migrationBuilder.DropTable(
                name: "FinancePlatform");
        }
    }
}
