using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Tests;

public class ProductoServiceTests
{
    private ProductoService CreateService() => new ProductoService();

    private Producto CreateProducto(string codigo = "PROD001", string nombre = "Producto Test", decimal precio = 100m)
    {
        return new Producto
        {
            Codigo = codigo,
            Nombre = nombre,
            Precio = precio
        };
    }

    #region Agregar Tests

    [Fact]
    public void Agregar_ProductoNuevo_RetornaExito()
    {
        // Arrange
        var service = CreateService();
        var producto = CreateProducto();

        // Act
        var (exito, mensaje) = service.Agregar(producto);

        // Assert
        Assert.True(exito);
        Assert.Equal("Producto agregado correctamente", mensaje);
    }

    [Fact]
    public void Agregar_ProductoNuevo_SeAgregaALaLista()
    {
        // Arrange
        var service = CreateService();
        var producto = CreateProducto();

        // Act
        service.Agregar(producto);
        var productos = service.ObtenerTodos();

        // Assert
        Assert.Single(productos);
        Assert.Contains(producto, productos);
    }

    [Fact]
    public void Agregar_ProductoDuplicado_RetornaError()
    {
        // Arrange
        var service = CreateService();
        var producto1 = CreateProducto("PROD001");
        var producto2 = CreateProducto("PROD001", "Otro Nombre", 200m);
        service.Agregar(producto1);

        // Act
        var (exito, mensaje) = service.Agregar(producto2);

        // Assert
        Assert.False(exito);
        Assert.Equal("Ya existe un producto con ese codigo", mensaje);
    }

    [Fact]
    public void Agregar_ProductoDuplicado_NoSeAgregaALaLista()
    {
        // Arrange
        var service = CreateService();
        var producto1 = CreateProducto("PROD001");
        var producto2 = CreateProducto("PROD001", "Otro Nombre", 200m);
        service.Agregar(producto1);

        // Act
        service.Agregar(producto2);
        var productos = service.ObtenerTodos();

        // Assert
        Assert.Single(productos);
    }

    [Fact]
    public void Agregar_MultiplesProductosUnicos_TodosSeAgregan()
    {
        // Arrange
        var service = CreateService();
        var producto1 = CreateProducto("PROD001");
        var producto2 = CreateProducto("PROD002");
        var producto3 = CreateProducto("PROD003");

        // Act
        service.Agregar(producto1);
        service.Agregar(producto2);
        service.Agregar(producto3);
        var productos = service.ObtenerTodos();

        // Assert
        Assert.Equal(3, productos.Count);
    }

    [Fact]
    public void Agregar_CodigosCaseSensitive_PermiteAmbos()
    {
        // Arrange
        var service = CreateService();
        var producto1 = CreateProducto("prod001");
        var producto2 = CreateProducto("PROD001");

        // Act
        var (exito1, _) = service.Agregar(producto1);
        var (exito2, _) = service.Agregar(producto2);

        // Assert
        Assert.True(exito1);
        Assert.True(exito2);
        Assert.Equal(2, service.ObtenerTodos().Count);
    }

    #endregion

    #region ObtenerTodos Tests

    [Fact]
    public void ObtenerTodos_SinProductos_RetornaListaVacia()
    {
        // Arrange
        var service = CreateService();

        // Act
        var productos = service.ObtenerTodos();

        // Assert
        Assert.NotNull(productos);
        Assert.Empty(productos);
    }

    [Fact]
    public void ObtenerTodos_ConProductos_RetornaTodosLosProductos()
    {
        // Arrange
        var service = CreateService();
        service.Agregar(CreateProducto("PROD001"));
        service.Agregar(CreateProducto("PROD002"));

        // Act
        var productos = service.ObtenerTodos();

        // Assert
        Assert.Equal(2, productos.Count);
    }

    [Fact]
    public void ObtenerTodos_RetornaProductosConDatosCorrectos()
    {
        // Arrange
        var service = CreateService();
        var productoOriginal = CreateProducto("TEST001", "Mi Producto", 150.50m);
        service.Agregar(productoOriginal);

        // Act
        var productos = service.ObtenerTodos();
        var productoObtenido = productos.First();

        // Assert
        Assert.Equal("TEST001", productoObtenido.Codigo);
        Assert.Equal("Mi Producto", productoObtenido.Nombre);
        Assert.Equal(150.50m, productoObtenido.Precio);
    }

    #endregion
}
