using EMDERSOFT.Data;
using EMDERSOFT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EMDERSOFT.Patterns.Facade;
using EMDERSOFT.Patterns.Strategy;
using EMDERSOFT.Patterns.Singleton;

namespace EMDERSOFT.Controllers;

[Authorize]
public class InventarioController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly HerramientaService _servicio;
    private readonly AppLogger _logger = AppLogger.ObtenerInstancia();

    public InventarioController(ApplicationDbContext context, HerramientaService servicio)
    {
        _context = context;
        _servicio = servicio;
    }

    // GET: Inventario
    public async Task<IActionResult> Index(string orden = "nombre_asc")
    {
        var herramientas = await _servicio.ObtenerTodasAsync();

        // Patrón Strategy: ordenamiento dinámico
        var estrategias = OrdenadorHerramientas.ObtenerEstrategias();
        var estrategiaSeleccionada = estrategias.ContainsKey(orden) ? estrategias[orden] : estrategias["nombre_asc"];
        
        var ordenador = new OrdenadorHerramientas(estrategiaSeleccionada);
        var herramientasOrdenadas = ordenador.Ejecutar(herramientas).ToList();

        ViewBag.OrdenActual = orden;
        ViewBag.EstrategiaActual = ordenador.EstrategiaActual;
        ViewBag.Estrategias = estrategias;

        _logger.Info($"Inventario consultado con orden: {ordenador.EstrategiaActual}");

        return View(herramientasOrdenadas);
    }

    // GET: Inventario/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var herramienta = await _servicio.ObtenerPorIdAsync(id.Value);

        if (herramienta == null) return NotFound();

        return View(herramienta);
    }

    // GET: Inventario/Create
    public IActionResult Create()
    {
        CargarCategorias();
        CargarEtiquetas();
        return View();
    }

    // POST: Inventario/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,Descripcion,CategoriaId,Cantidad,Estado,Ubicacion")]
        Herramienta herramienta,
        int[] etiquetasSeleccionadas)
    {
        if (string.IsNullOrWhiteSpace(herramienta.Descripcion))
            herramienta.Descripcion = string.Empty;

        ModelState.Clear();
        TryValidateModel(herramienta);

        if (ModelState.IsValid)
        {
            var resultado = await _servicio.CrearAsync(herramienta, etiquetasSeleccionadas);
            TempData["Mensaje"] = resultado.Exitoso ? resultado.Mensaje : resultado.Error;
            
            if (resultado.Exitoso)
                return RedirectToAction(nameof(Index));
        }

        CargarCategorias(herramienta.CategoriaId);
        CargarEtiquetas(etiquetasSeleccionadas);
        return View(herramienta);
    }

    // GET: Inventario/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var herramienta = await _context.Herramientas
            .Include(h => h.Etiquetas)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (herramienta == null) return NotFound();

        CargarCategorias(herramienta.CategoriaId);
        CargarEtiquetas(herramienta.Etiquetas.Select(e => e.Id).ToArray());
        return View(herramienta);
    }

    // POST: Inventario/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Nombre,Descripcion,CategoriaId,Cantidad,Estado,Ubicacion,FechaRegistro")]
        Herramienta herramienta,
        int[] etiquetasSeleccionadas)
    {
        if (id != herramienta.Id) return NotFound();

        if (string.IsNullOrWhiteSpace(herramienta.Descripcion))
            herramienta.Descripcion = string.Empty;

        ModelState.Clear();
        TryValidateModel(herramienta);

        if (ModelState.IsValid)
        {
            var resultado = await _servicio.ActualizarAsync(herramienta, etiquetasSeleccionadas);
            if (!resultado.Exitoso && !_servicio.Existe(herramienta.Id))
            {
                return NotFound();
            }

            TempData["Mensaje"] = resultado.Exitoso ? resultado.Mensaje : resultado.Error;
            return RedirectToAction(nameof(Index));
        }

        CargarCategorias(herramienta.CategoriaId);
        CargarEtiquetas(etiquetasSeleccionadas);
        return View(herramienta);
    }

    // GET: Inventario/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var herramienta = await _context.Herramientas
            .Include(h => h.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (herramienta == null) return NotFound();

        return View(herramienta);
    }

    // POST: Inventario/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var resultado = await _servicio.EliminarAsync(id);
        TempData["Mensaje"] = resultado.Exitoso ? resultado.Mensaje : resultado.Error;
        
        return RedirectToAction(nameof(Index));
    }

    // Método auxiliar para cargar el select de Categorias
    private void CargarCategorias(int? selectedId = null)
    {
        ViewBag.CategoriaId = new SelectList(
            _context.Categorias.OrderBy(c => c.Nombre),
            "Id",       // valor del <option>
            "Nombre",   // texto visible
            selectedId  // opción seleccionada por defecto
        );
    }
    private void CargarEtiquetas(int[]? seleccionadas = null)
    {
        ViewBag.Etiquetas = _context.Etiquetas
            .OrderBy(e => e.Nombre)
            .Select(e => new
            {
                e.Id,
                e.Nombre,
                Seleccionada = seleccionadas != null && seleccionadas.Contains(e.Id)
            })
            .ToList();
    }
    private bool HerramientaExists(int id)
        => _context.Herramientas.Any(e => e.Id == id);
}