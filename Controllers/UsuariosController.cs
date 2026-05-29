using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Data;
using EMDERSOFT.Models;

namespace EMDERSOFT.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController(ApplicationDbContext context) : Controller
{
    // Muestra la lista de todos los usuarios registrados
    public async Task<IActionResult> Index()
    {
        return View(await context.Usuarios.ToListAsync());
    }

    // Muestra el formulario para crear un nuevo usuario
    public IActionResult Create()
    {
        return View();
    }

    // Recibe los datos del formulario y guarda el nuevo usuario en la base de datos
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Username,Password,Rol")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            context.Add(usuario);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // Muestra el formulario para editar un usuario existente según su ID
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }

    // Recibe los datos modificados y actualiza el usuario en la base de datos
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Rol")] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(usuario);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // Muestra la pantalla de confirmación para eliminar un usuario
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // Ejecuta la eliminación definitiva del usuario en la base de datos
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Verifica si un usuario existe en la base de datos
    private bool UsuarioExists(int id)
    {
        return context.Usuarios.Any(e => e.Id == id);
    }
}
