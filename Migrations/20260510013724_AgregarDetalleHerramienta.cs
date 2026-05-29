using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMDERSOFT.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDetalleHerramienta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HerramientaEtiqueta_Etiqueta_EtiquetasId",
                table: "HerramientaEtiqueta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Etiqueta",
                table: "Etiqueta");

            migrationBuilder.RenameTable(
                name: "Etiqueta",
                newName: "Etiquetas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Etiquetas",
                table: "Etiquetas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DetallesHerramienta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(8,3)", nullable: true),
                    GarantiaMeses = table.Column<int>(type: "int", nullable: true),
                    Especificaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HerramientaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesHerramienta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesHerramienta_Herramientas_HerramientaId",
                        column: x => x.HerramientaId,
                        principalTable: "Herramientas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesHerramienta_HerramientaId",
                table: "DetallesHerramienta",
                column: "HerramientaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HerramientaEtiqueta_Etiquetas_EtiquetasId",
                table: "HerramientaEtiqueta",
                column: "EtiquetasId",
                principalTable: "Etiquetas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HerramientaEtiqueta_Etiquetas_EtiquetasId",
                table: "HerramientaEtiqueta");

            migrationBuilder.DropTable(
                name: "DetallesHerramienta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Etiquetas",
                table: "Etiquetas");

            migrationBuilder.RenameTable(
                name: "Etiquetas",
                newName: "Etiqueta");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Etiqueta",
                table: "Etiqueta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HerramientaEtiqueta_Etiqueta_EtiquetasId",
                table: "HerramientaEtiqueta",
                column: "EtiquetasId",
                principalTable: "Etiqueta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
