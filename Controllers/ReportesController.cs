using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Data;
using EMDERSOFT.Patterns.Builder;
using EMDERSOFT.Patterns.Singleton;
using Microsoft.AspNetCore.Authorization;

namespace EMDERSOFT.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AppLogger _logger = AppLogger.ObtenerInstancia();

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Completo()
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Categoria)
                .ToListAsync();

            var reporte = new ReporteHerramientasBuilder(herramientas)
                .ConEncabezado("Reporte Completo de Herramientas")
                .ConHerramientasMasAbundantes(5)
                .ConHerramientasSinStock()
                .ConResumenPorCategoria()
                .ConResumenPorEstado()
                .Construir();

            _logger.Info("Reporte completo generado");
            return View("Reporte", reporte);
        }

        public async Task<IActionResult> Resumen()
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Categoria)
                .ToListAsync();

            var reporte = new ReporteHerramientasBuilder(herramientas)
                .ConEncabezado("Resumen Ejecutivo")
                .ConResumenPorCategoria()
                .ConResumenPorEstado()
                .Construir();

            _logger.Info("Reporte resumen generado");
            return View("Reporte", reporte);
        }

        public async Task<IActionResult> Inventario()
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Categoria)
                .ToListAsync();

            var reporte = new ReporteHerramientasBuilder(herramientas)
                .ConEncabezado("Reporte de Inventario Crítico")
                .ConHerramientasSinStock()
                .Construir();

            _logger.Info("Reporte de inventario generado");
            return View("Reporte", reporte);
        }
    }
}
