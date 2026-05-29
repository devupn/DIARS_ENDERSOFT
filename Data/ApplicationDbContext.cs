using Microsoft.EntityFrameworkCore;
using EMDERSOFT.Models;

namespace EMDERSOFT.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options)
{
    public DbSet<Herramienta> Herramientas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Etiqueta> Etiquetas { get; set; }  
    public DbSet<DetalleHerramienta> DetallesHerramienta { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación 1:N
        // Herramienta tiene UNA Categoria
        // Categoria tiene MUCHAS Herramientas
        modelBuilder.Entity<Herramienta>()
            .HasOne(h => h.Categoria)
            .WithMany(c => c.Herramientas)
            .HasForeignKey(h => h.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        // ↑ Restrict: no puedes borrar una Categoría
        //   si tiene herramientas asociadas

        
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Electricidad" },
            new Categoria { Id = 2, Nombre = "Mecánica" },
            new Categoria { Id = 3, Nombre = "Corte" },
            new Categoria { Id = 4, Nombre = "Medición" }
        );
        // Relación N:M
        modelBuilder.Entity<Herramienta>()
            .HasMany(h => h.Etiquetas)
            .WithMany(e => e.Herramientas)
            .UsingEntity(j => j.ToTable("HerramientaEtiqueta"));

        // Datos semilla para Etiquetas
        modelBuilder.Entity<Etiqueta>().HasData(
            new Etiqueta { Id = 1, Nombre = "En préstamo" },
            new Etiqueta { Id = 2, Nombre = "En mantenimiento" },
            new Etiqueta { Id = 3, Nombre = "Crítico" },
            new Etiqueta { Id = 4, Nombre = "Nuevo" }
        );
        // Relación 1:1
        modelBuilder.Entity<Herramienta>()
            .HasOne(h => h.Detalle)
            .WithOne(d => d.Herramienta)
            .HasForeignKey<DetalleHerramienta>(d => d.HerramientaId)
            .OnDelete(DeleteBehavior.Cascade);
            }
}