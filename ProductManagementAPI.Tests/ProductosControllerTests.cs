using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Controllers;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Tests;

public class ProductosControllerTests
{
    private ProductosController CreateController(ProductoService? service = null)
    {
        return new ProductosController(service ?? new ProductoService());
    }

    private ProductoDto CreateProductoDto(string codigo = "PROD001", string nombre = "Producto Test", decimal precio = 100m)
    {
        return new ProductoDto
        {
            Codigo = codigo,
            Nombre = nombre,
            Precio = precio
        };
    }

    #region Agregar (POST) Tests

    [Fact]
    public void Agregar_ProductoValido_RetornaCreatedAtAction()
    {
        // Arrange
        var controller = CreateController();
        var dto = CreateProductoDto();

        // Act
        var result = controller.Agregar(dto);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public void Agregar_ProductoValido_RetornaProductoCreado()
    {
        // Arrange
        var controller = CreateController();
        var dto = CreateProductoDto("TEST001", "Mi Producto", 250m);

        // Act
        var result = controller.Agregar(dto) as CreatedAtActionResult;
        var producto = result?.Value as Producto;

        // Assert
        Assert.NotNull(producto);
        Assert.Equal("TEST001", producto.Codigo);
        Assert.Equal("Mi Producto", producto.Nombre);
        Assert.Equal(250m, producto.Precio);
    }

    [Fact]
    public void Agregar_ProductoDuplicado_RetornaConflict()
    {
        // Arrange
        var service = new ProductoService();
        var controller = CreateController(service);
        var dto = CreateProductoDto("PROD001");
        controller.Agregar(dto);

        // Act
        var result = controller.Agregar(dto);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public void Agregar_ProductoDuplicado_RetornaMensajeError()
    {
        // Arrange
        var service = new ProductoService();
        var controller = CreateController(service);
        var dto = CreateProductoDto("PROD001");
        controller.Agregar(dto);

        // Act
        var result = controller.Agregar(dto) as ConflictObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
    }

    #endregion

    #region ObtenerTodos (GET) Tests

    [Fact]
    public void ObtenerTodos_SinProductos_RetornaOkConListaVacia()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.ObtenerTodos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var productos = Assert.IsType<List<Producto>>(okResult.Value);
        Assert.Empty(productos);
    }

    [Fact]
    public void ObtenerTodos_ConProductos_RetornaOkConProductos()
    {
        // Arrange
        var service = new ProductoService();
        var controller = CreateController(service);
        controller.Agregar(CreateProductoDto("PROD001"));
        controller.Agregar(CreateProductoDto("PROD002"));

        // Act
        var result = controller.ObtenerTodos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var productos = Assert.IsType<List<Producto>>(okResult.Value);
        Assert.Equal(2, productos.Count);
    }

    [Fact]
    public void ObtenerTodos_RetornaStatusCode200()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.ObtenerTodos() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AgregarYObtener_ProductoAgregado_AparecEnLista()
    {
        // Arrange
        var service = new ProductoService();
        var controller = CreateController(service);
        var dto = CreateProductoDto("INT001", "Producto Integracion", 500m);

        // Act
        controller.Agregar(dto);
        var result = controller.ObtenerTodos() as OkObjectResult;
        var productos = result?.Value as List<Producto>;

        // Assert
        Assert.NotNull(productos);
        Assert.Single(productos);
        Assert.Equal("INT001", productos[0].Codigo);
    }

    [Fact]
    public void AgregarMultiples_TodosAparecenEnLista()
    {
        // Arrange
        var service = new ProductoService();
        var controller = CreateController(service);

        // Act
        controller.Agregar(CreateProductoDto("PROD001"));
        controller.Agregar(CreateProductoDto("PROD002"));
        controller.Agregar(CreateProductoDto("PROD003"));
        var result = controller.ObtenerTodos() as OkObjectResult;
        var productos = result?.Value as List<Producto>;

        // Assert
        Assert.NotNull(productos);
        Assert.Equal(3, productos.Count);
    }

    #endregion
}
