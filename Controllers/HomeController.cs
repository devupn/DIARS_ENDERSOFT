using System.Diagnostics;
using EMDERSOFT.Models;
using EMDERSOFT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Patterns.Singleton;

namespace EMDERSOFT.Controllers;

[Authorize]
public class HomeController(ApplicationDbContext context) : Controller
{

    // Muestra el tablero principal con los indicadores clave
    public async Task<IActionResult> Index()
    {
        // Total de bienes
        var totalBienes = await context.Herramientas.CountAsync();

        // Bienes registrados este mes
        var fechaActual = DateTime.Now;
        var nuevosEsteMes = await context.Herramientas
            .CountAsync(h => h.FechaRegistro.Month == fechaActual.Month && h.FechaRegistro.Year == fechaActual.Year);

        // Total de usuarios
        var totalResponsables = await context.Usuarios.CountAsync();

        // Enviar datos a la vista
        ViewBag.TotalBienes = totalBienes;
        ViewBag.NuevosEsteMes = nuevosEsteMes;
        ViewBag.TotalResponsables = totalResponsables;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Patrón Singleton: muestra los últimos registros del log del sistema
    public IActionResult Log()
    {
        var registros = AppLogger.ObtenerInstancia().ObtenerUltimosRegistros(50);
        return View(registros);
    }

    // Captura los errores globales y muestra una vista genérica de error
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
