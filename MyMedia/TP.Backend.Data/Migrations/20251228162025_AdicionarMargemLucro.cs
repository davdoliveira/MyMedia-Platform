using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMargemLucro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MargemLucro",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MargemLucro",
                table: "Produtos");
        }
    }
}
