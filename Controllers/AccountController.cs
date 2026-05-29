using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMDERSOFT.Data;

using Microsoft.AspNetCore.Authorization;

namespace EMDERSOFT.Controllers;

[AllowAnonymous]
public class AccountController(ApplicationDbContext context) : Controller
{

    // Muestra la vista del formulario de inicio de sesión
    [HttpGet]
    public IActionResult Login()
    {
        // Si ya inició sesión, ir al inicio
        if (User.Identity?.IsAuthenticated is true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // Procesa las credenciales del usuario e inicia la sesión si son correctas
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = context.Usuarios.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View();
    }

    // Cierra la sesión activa del usuario y redirige al login
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
