using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Data;
using EMDERSOFT.Models;
using EMDERSOFT.Patterns.Singleton;

namespace EMDERSOFT.Patterns.Facade
{
    public class HerramientaService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppLogger _logger;

        public HerramientaService(ApplicationDbContext context)
        {
            _context = context;
            _logger = AppLogger.ObtenerInstancia();
        }

        public async Task<List<Herramienta>> ObtenerTodasAsync()
        {
            _logger.Info("Consultando listado de herramientas");
            return await _context.Herramientas
                .Include(h => h.Categoria)
                .Include(h => h.Etiquetas)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();
        }

        public async Task<Herramienta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Herramientas
                .Include(h => h.Categoria)
                .Include(h => h.Etiquetas)
                .Include(h => h.Detalle)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<ResultadoOperacion> CrearAsync(Herramienta herramienta, int[]? etiquetasSeleccionadas = null)
        {
            try
            {
                herramienta.FechaRegistro = DateTime.Now;

                if (etiquetasSeleccionadas != null && etiquetasSeleccionadas.Length > 0)
                {
                    herramienta.Etiquetas = await _context.Etiquetas
                        .Where(e => etiquetasSeleccionadas.Contains(e.Id))
                        .ToListAsync();
                }

                _context.Add(herramienta);
                await _context.SaveChangesAsync();

                _logger.Info($"Herramienta creada: '{herramienta.Nombre}' Id={herramienta.Id}");
                return ResultadoOperacion.Exito($"Herramienta '{herramienta.Nombre}' creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear herramienta: {ex.Message}");
                return ResultadoOperacion.Fallo("Ocurrió un error al guardar la herramienta.");
            }
        }

        public async Task<ResultadoOperacion> ActualizarAsync(Herramienta herramienta, int[]? etiquetasSeleccionadas = null)
        {
            try
            {
                var herramientaExistente = await _context.Herramientas
                    .Include(h => h.Etiquetas)
                    .FirstOrDefaultAsync(h => h.Id == herramienta.Id);

                if (herramientaExistente == null)
                    return ResultadoOperacion.Fallo("Herramienta no encontrada.");

                // Actualizar campos simples
                herramientaExistente.Nombre = herramienta.Nombre;
                herramientaExistente.Descripcion = herramienta.Descripcion;
                herramientaExistente.CategoriaId = herramienta.CategoriaId;
                herramientaExistente.Cantidad = herramienta.Cantidad;
                herramientaExistente.Estado = herramienta.Estado;
                herramientaExistente.Ubicacion = herramienta.Ubicacion;

                // Reemplazar etiquetas
                herramientaExistente.Etiquetas.Clear();
                if (etiquetasSeleccionadas != null && etiquetasSeleccionadas.Length > 0)
                {
                    var nuevas = await _context.Etiquetas
                        .Where(e => etiquetasSeleccionadas.Contains(e.Id))
                        .ToListAsync();
                    foreach (var etiqueta in nuevas)
                        herramientaExistente.Etiquetas.Add(etiqueta);
                }

                await _context.SaveChangesAsync();

                _logger.Info($"Herramienta actualizada: Id={herramienta.Id}");
                return ResultadoOperacion.Exito($"Herramienta actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar: {ex.Message}");
                return ResultadoOperacion.Fallo("Ocurrió un error al actualizar.");
            }
        }

        public async Task<ResultadoOperacion> EliminarAsync(int id)
        {
            try
            {
                var herramienta = await _context.Herramientas.FindAsync(id);
                if (herramienta == null)
                    return ResultadoOperacion.Fallo("Herramienta no encontrada.");

                _context.Herramientas.Remove(herramienta);
                await _context.SaveChangesAsync();

                _logger.Advertencia($"Herramienta eliminada: Id={id}, Nombre='{herramienta.Nombre}'");
                return ResultadoOperacion.Exito("Herramienta eliminada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al eliminar: {ex.Message}");
                return ResultadoOperacion.Fallo("No se pudo eliminar la herramienta.");
            }
        }

        public bool Existe(int id) => _context.Herramientas.Any(e => e.Id == id);
    }
}
