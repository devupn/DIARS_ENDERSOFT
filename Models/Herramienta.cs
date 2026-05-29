using System.ComponentModel.DataAnnotations;

namespace EMDERSOFT.Models;

public class Herramienta
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    public required string Nombre { get; set; }

    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes seleccionar una categoría.")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    // Objeto completo — solo disponible con .Include()
    public Categoria? Categoria { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Display(Name = "Cantidad")]
    [Range(0, 9999)]
    public int Cantidad { get; set; }

    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Disponible";

    [Display(Name = "Ubicación")]
    public string Ubicacion { get; set; } = string.Empty;

    [Display(Name = "Fecha de Registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
   
    public ICollection<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();
    public DetalleHerramienta? Detalle { get; set; }
}