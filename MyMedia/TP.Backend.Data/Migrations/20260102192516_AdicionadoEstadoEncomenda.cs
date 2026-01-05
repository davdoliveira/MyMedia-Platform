using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoEstadoEncomenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Encomendas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Encomendas");
        }
    }
}
