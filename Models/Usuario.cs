using System.ComponentModel.DataAnnotations;

namespace EMDERSOFT.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    public string Rol { get; set; } = "Administrador";
}
