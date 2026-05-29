using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Data;
using EMDERSOFT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMDERSOFT.Controllers;

[Authorize]
public class DetallesController(ApplicationDbContext context) : Controller
{
    // GET: /Detalles/Editar/5  (5 = HerramientaId)
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var detalle = await context.DetallesHerramienta
            .Include(d => d.Herramienta)
            .FirstOrDefaultAsync(d => d.HerramientaId == id);

        if (detalle == null)
        {
            // No existe aún → crear uno vacío
            var herramienta = await context.Herramientas.FindAsync(id);
            if (herramienta == null) return NotFound();

            detalle = new DetalleHerramienta
            {
                HerramientaId = (int)id,
                Herramienta = herramienta
            };
        }

        return View(detalle);
    }

    // POST: /Detalles/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id,
        [Bind("Id,HerramientaId,Marca,Modelo,Peso,GarantiaMeses,Especificaciones")]
        DetalleHerramienta detalle)
    {
        if (id != detalle.HerramientaId) return NotFound();

        ModelState.Clear();
        TryValidateModel(detalle);

        if (ModelState.IsValid)
    {
        var existente = await context.DetallesHerramienta
            .FirstOrDefaultAsync(d => d.HerramientaId == id);

        if (existente == null)
        {
            // Nuevo → insertar
            var nuevo = new DetalleHerramienta
            {
                HerramientaId = id,
                Marca = detalle.Marca,
                Modelo = detalle.Modelo,
                Peso = detalle.Peso,
                GarantiaMeses = detalle.GarantiaMeses,
                Especificaciones = detalle.Especificaciones
            };
            context.DetallesHerramienta.Add(nuevo);
        }
        else
        {
            // Ya existe → modificar propiedades directamente (sin Update)
            existente.Marca = detalle.Marca;
            existente.Modelo = detalle.Modelo;
            existente.Peso = detalle.Peso;
            existente.GarantiaMeses = detalle.GarantiaMeses;
            existente.Especificaciones = detalle.Especificaciones;
            // NO llamar context.Update() — EF Core ya lo trackea
        }

        await context.SaveChangesAsync();
        TempData["Mensaje"] = "Detalle guardado correctamente.";
        return RedirectToAction("Details", "Inventario", new { id = detalle.HerramientaId });
    }

        return View(detalle);
    }
}