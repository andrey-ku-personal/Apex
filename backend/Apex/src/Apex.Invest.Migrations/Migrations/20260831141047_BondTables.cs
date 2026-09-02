using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Apex.Invest.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class BondTables : Migration
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
                    Ticker = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Issuer = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
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
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    ParPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CouponRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PaymentFrequence = table.Column<int>(type: "integer", nullable: false),
                    NextCouponDate = table.Column<int>(type: "integer", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "date", nullable: false)
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
                name: "BondOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BondId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BondOperation_Bond_BondId",
                        column: x => x.BondId,
                        principalTable: "Bond",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FinancePlatform",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Аигенис" },
                    { 2, "Беларус Банк" },
                    { 3, "Альфа Банк" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondOperation_BondId",
                table: "BondOperation",
                column: "BondId");

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
                name: "BondOperation");

            migrationBuilder.DropTable(
                name: "Bond");

            migrationBuilder.DropTable(
                name: "FinanceInstrument");

            migrationBuilder.DropTable(
                name: "FinancePlatform");
        }
    }
}
