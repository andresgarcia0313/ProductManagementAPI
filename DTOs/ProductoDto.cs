using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.DTOs;

public class ProductoDto
{
    [Required(ErrorMessage = "El codigo es requerido")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }
}
