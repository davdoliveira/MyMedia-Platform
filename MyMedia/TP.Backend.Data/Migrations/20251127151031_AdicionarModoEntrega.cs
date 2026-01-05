using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarModoEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModoEntregaId",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ModosEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModosEntrega", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_ModoEntregaId",
                table: "Produtos",
                column: "ModoEntregaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_ModosEntrega_ModoEntregaId",
                table: "Produtos",
                column: "ModoEntregaId",
                principalTable: "ModosEntrega",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_ModosEntrega_ModoEntregaId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "ModosEntrega");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_ModoEntregaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ModoEntregaId",
                table: "Produtos");
        }
    }
}
