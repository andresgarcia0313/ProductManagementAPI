using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services;

public class ProductoService
{
    private readonly List<Producto> _productos = new();

    public (bool exito, string mensaje) Agregar(Producto producto)
    {
        if (_productos.Any(p => p.Codigo == producto.Codigo))
            return (false, "Ya existe un producto con ese codigo");

        _productos.Add(producto);
        return (true, "Producto agregado correctamente");
    }

    public List<Producto> ObtenerTodos() => _productos;
}
