using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Builder
{
    public class ReporteHerramientasBuilder
    {
        private readonly ReporteHerramientas _reporte = new ReporteHerramientas();
        private readonly List<Herramienta> _herramientas;

        public ReporteHerramientasBuilder(List<Herramienta> herramientas)
        {
            _herramientas = herramientas;
        }

        public ReporteHerramientasBuilder ConEncabezado(string titulo)
        {
            _reporte.Titulo = titulo;
            _reporte.TotalHerramientas = _herramientas.Count;
            _reporte.TotalCantidadStock = _herramientas.Sum(h => h.Cantidad);
            _reporte.SeccionesIncluidas.Add("Estadísticas generales");
            return this;
        }

        public ReporteHerramientasBuilder ConHerramientasMasAbundantes(int cantidad = 5)
        {
            _reporte.HerramientasMasAbundantes = _herramientas
                .OrderByDescending(h => h.Cantidad)
                .Take(cantidad)
                .ToList();
            _reporte.SeccionesIncluidas.Add($"Top {cantidad} herramientas más abundantes");
            return this;
        }

        public ReporteHerramientasBuilder ConHerramientasSinStock()
        {
            _reporte.HerramientasSinStock = _herramientas
                .Where(h => h.Cantidad == 0)
                .OrderBy(h => h.Nombre)
                .ToList();
            _reporte.SeccionesIncluidas.Add("Herramientas sin stock (Cantidad 0)");
            return this;
        }

        public ReporteHerramientasBuilder ConResumenPorCategoria()
        {
            _reporte.ResumenPorCategoria = _herramientas
                .Where(h => h.Categoria != null && !string.IsNullOrEmpty(h.Categoria.Nombre))
                .GroupBy(h => h.Categoria!.Nombre)
                .ToDictionary(g => g.Key, g => g.Count());
            _reporte.SeccionesIncluidas.Add("Resumen por categoría");
            return this;
        }

        public ReporteHerramientasBuilder ConResumenPorEstado()
        {
            _reporte.ResumenPorEstado = _herramientas
                .Where(h => !string.IsNullOrEmpty(h.Estado))
                .GroupBy(h => h.Estado)
                .ToDictionary(g => g.Key, g => g.Count());
            _reporte.SeccionesIncluidas.Add("Resumen por estado");
            return this;
        }

        public ReporteHerramientas Construir()
        {
            _reporte.FechaGeneracion = DateTime.Now;
            return _reporte;
        }
    }
}
