using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMDERSOFT.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEtiquetasRelacionNM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Etiqueta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiqueta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HerramientaEtiqueta",
                columns: table => new
                {
                    EtiquetasId = table.Column<int>(type: "int", nullable: false),
                    HerramientasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HerramientaEtiqueta", x => new { x.EtiquetasId, x.HerramientasId });
                    table.ForeignKey(
                        name: "FK_HerramientaEtiqueta_Etiqueta_EtiquetasId",
                        column: x => x.EtiquetasId,
                        principalTable: "Etiqueta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HerramientaEtiqueta_Herramientas_HerramientasId",
                        column: x => x.HerramientasId,
                        principalTable: "Herramientas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Etiqueta",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "En préstamo" },
                    { 2, "En mantenimiento" },
                    { 3, "Crítico" },
                    { 4, "Nuevo" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HerramientaEtiqueta_HerramientasId",
                table: "HerramientaEtiqueta",
                column: "HerramientasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HerramientaEtiqueta");

            migrationBuilder.DropTable(
                name: "Etiqueta");
        }
    }
}
