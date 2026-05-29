using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Builder
{
    public class ReporteHerramientas
    {
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public string GeneradoPor { get; set; } = "Sistema EMDERSOFT";

        public int TotalHerramientas { get; set; }
        public int TotalCantidadStock { get; set; }

        public List<Herramienta>? HerramientasMasAbundantes { get; set; }
        public List<Herramienta>? HerramientasSinStock { get; set; }
        public Dictionary<string, int>? ResumenPorCategoria { get; set; }
        public Dictionary<string, int>? ResumenPorEstado { get; set; }

        public List<string> SeccionesIncluidas { get; set; } = new List<string>();
    }
}
