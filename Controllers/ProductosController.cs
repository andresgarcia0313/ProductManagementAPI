using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers;

[ApiController]
[Route("api/productos")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpPost]
    public IActionResult Agregar([FromBody] ProductoDto dto)
    {
        var producto = new Producto
        {
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Precio = dto.Precio
        };

        var (exito, mensaje) = _productoService.Agregar(producto);

        if (!exito)
            return Conflict(new { message = mensaje });

        return CreatedAtAction(nameof(ObtenerTodos), producto);
    }

    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        return Ok(_productoService.ObtenerTodos());
    }
}
