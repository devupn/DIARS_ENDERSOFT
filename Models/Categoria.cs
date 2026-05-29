using System.ComponentModel.DataAnnotations;

namespace EMDERSOFT.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Nombre de Categoría")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    // Propiedad de navegación
    // Una Categoría tiene MUCHAS Herramientas
    public ICollection<Herramienta> Herramientas { get; set; } 
        = new List<Herramienta>();
}