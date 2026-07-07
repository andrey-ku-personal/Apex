using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Invest.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class _02_AddFinancePlatforms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""FinancePlatform"" (""Name"")
                VALUES ('Aigenis'), ('Беларус Банк'), ('Альфа Банк')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""FinancePlatform""
            ");
        }
    }
}
