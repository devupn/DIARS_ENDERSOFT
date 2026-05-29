using EMDERSOFT.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using EMDERSOFT.Models;

var builder = WebApplication.CreateBuilder(args);


// Configurar la conexión a la base de datos SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection")));

// Configurar la autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Requerir que todos los usuarios estén autenticados por defecto en toda la aplicación
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Agregar soporte para controladores y vistas (MVC)
builder.Services.AddControllersWithViews();

// Registrar el servicio Facade para inyección de dependencias
builder.Services.AddScoped<EMDERSOFT.Patterns.Facade.HerramientaService>();

var app = builder.Build();

// Crear usuario por defecto
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Migración automática

    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new Usuario { Username = "admin", Password = "123", Rol = "Administrador" });
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Forzar el uso de HTTPS y servir archivos estáticos (CSS, JS, imágenes)
app.UseHttpsRedirection();
app.UseStaticFiles();

// Habilitar el sistema de rutas
app.UseRouting();

// Activar los middlewares de seguridad
app.UseAuthentication();
app.UseAuthorization();

// Configurar la ruta principal por defecto (Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();