using System.ComponentModel.DataAnnotations;

namespace EMDERSOFT.Models;

public class Etiqueta
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio.")]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "Etiqueta")]
    public string Nombre { get; set; } = string.Empty;

    // Una Etiqueta pertenece a MUCHAS Herramientas
    public ICollection<Herramienta> Herramientas { get; set; } = new List<Herramienta>();
}