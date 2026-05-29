using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMDERSOFT.Models;

public class DetalleHerramienta
{
    public int Id { get; set; }

    [StringLength(200)]
    [Display(Name = "Marca")]
    public string? Marca { get; set; }

    [StringLength(100)]
    [Display(Name = "Modelo")]
    public string? Modelo { get; set; }

    [Display(Name = "Peso (kg)")]
    [Column(TypeName = "decimal(8,3)")]
    public decimal? Peso { get; set; }

    [Display(Name = "Garantía (meses)")]
    public int? GarantiaMeses { get; set; }

    [StringLength(500)]
    [Display(Name = "Especificaciones técnicas")]
    public string? Especificaciones { get; set; }

    // Clave foránea hacia Herramienta
    public int HerramientaId { get; set; }

    // Navegación hacia la Herramienta
    public Herramienta? Herramienta { get; set; }
}